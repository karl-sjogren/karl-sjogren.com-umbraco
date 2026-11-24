import { html, property, customElement, state, nothing, unsafeSVG, unsafeCSS } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import type { UmbModalExtensionElement } from "@umbraco-cms/backoffice/modal";
import type { IconPickerSettingsDialogData, IconPickerSettingsDialogValue } from "./dialog.token.ts";
import { allIcons, loadIconLibrary, type Icon } from "./icons.ts";
import { findIconDefinition, icon as svgIcon } from '@fortawesome/fontawesome-svg-core';

import styles from './dialog.scss?inline';

@customElement('umbraco-blog-icon-picker-dialog')
export default class IconPickerDialogElement
        extends UmbLitElement
        implements UmbModalExtensionElement<IconPickerSettingsDialogData, IconPickerSettingsDialogValue> {

    @property({ attribute: false }) modalContext?: UmbModalExtensionElement<IconPickerSettingsDialogData, IconPickerSettingsDialogValue>["modalContext"];
    @property({ attribute: false }) data?: IconPickerSettingsDialogData;

    @state() private _query = '';
    @state() private _page = 1;
    @state() private pageSize = 20;
    @state() private _selected : Icon | undefined;
    @state() private _current: Icon[] = [];

    #initializedData?: IconPickerSettingsDialogData;

    connectedCallback(): void {
        super.connectedCallback();

        loadIconLibrary();
    }

    protected firstUpdated(): void {
        this.#initializeData();
    }

    protected updated(changed: Map<PropertyKey, unknown>): void {
        if(changed.has("data")) {
            this.#initializeData();
        }
    }

    #initializeData() {
        console.log('Initializing data with', this.data);
        if(!this.data || this.#initializedData === this.data) {
            return;
        }

        this.#initializedData = this.data;
        const prefix = this.data?.prefix;
        const iconName = this.data?.iconName;
        if (prefix && iconName) {
            this._selected = allIcons.find(icon => icon.prefix === prefix && icon.iconName === iconName);
        }

        void this.#search();
    }

    disconnectedCallback(): void {
        super.disconnectedCallback();
    }

    async #search() {
        const lowerCaseQuery = this._query.toLowerCase();
        this._current = allIcons.filter(icon => icon.searchableNames.some(name => name.includes(lowerCaseQuery)));
    }

    #queryChanged(event: Event) {
        this._query = (event.target as HTMLInputElement).value;
        this._page = 1;
        void this.#search();
    }

    #pageChanged(event: Event) {
        this._page = (event.target as HTMLElement & { current: number }).current;
        void this.#search();
    }

    #chooseIcon(icon: Icon) {
        this._selected = icon;
    }

    #submit() {
        const icon = this._selected;
        if(!icon) {
            return;
        }

        this.modalContext?.setValue(icon);
        this.modalContext?.submit();
    }

    #renderPreview(icon: Icon) {
        const lookup = findIconDefinition({ prefix: icon.prefix, iconName: icon.iconName })
        const svg = svgIcon(lookup);
        const htmlString = [...svg.node].map(node => node.outerHTML).join('');
        return html`<div class="icon-preview">${unsafeSVG(htmlString)}</div>`;
    }

    #renderResultMetadata(icon: Icon) {
        const metadata: string[] = [];

        let prefixLabel = 'Unknown';
        if(icon.prefix === 'fas') {
            prefixLabel = 'Solid';
        } else if(icon.prefix === 'far') {
            prefixLabel = 'Regular';
        } else if(icon.prefix === 'fab') {
            prefixLabel = 'Brand';
        }

        if(icon.prefix) {
            metadata.push(`${prefixLabel}`);
        }

        return metadata.length ? html`<small>${metadata.join(" · ")}</small>` : nothing;
    }

    #renderResults() {
        if (!this._current.length) {
            return html`<p class="empty">No icons match the current search and filters.</p>`;
        }

        const totalPages = Math.max(1, Math.ceil(this._current.length / this.pageSize));
        const currentPage = Math.min(this._page, totalPages);
        const currentIcons = this._current.slice((currentPage - 1) * this.pageSize, currentPage * this.pageSize);

        return html`
            <p class="count" aria-live="polite">${this._current.length} icons</p>
            <div class="icon-grid">
                ${currentIcons.map(
                    (icon) =>
                        html`
                            <button
                            class=${this._selected === icon ? "icon-card selected" : "icon-card"}
                            type="button"
                            @click=${() => this.#chooseIcon(icon)}
                            aria-label=${"Choose " + icon.iconName}
                            >
                                <span class="preview">${this.#renderPreview(icon)}</span>
                                <strong>${icon.iconName}</strong>
                                ${this.#renderResultMetadata(icon)}
                            </button>
                    `)
                }
            </div>
            ${totalPages > 1
                ? html`<uui-pagination
                    label="Icon search pages"
                    .total=${totalPages}
                    .current=${currentPage}
                    @change=${this.#pageChanged}
                ></uui-pagination>` : nothing}
        `;
    }

    render() {
        return html`
            <umb-body-layout headline=${"Choose an icon"}>
                <div class="toolbar">
                    <uui-input
                        type="search"
                        label="Search icons"
                        placeholder="Search icons"
                        .value=${this._query}
                        @input=${this.#queryChanged}
                        >
                        <uui-icon name="icon-search" slot="prepend"></uui-icon>
                    </uui-input>
                </div>
                <div class="layout">
                    <main>${this.#renderResults()}</main>
                </div>
                <div slot="actions">
                    <uui-button label="Choose" look="primary" color="positive" ?disabled=${!this._selected} @click=${() => this.#submit()}>Choose</uui-button>
                    <uui-button label="Cancel" @click=${() => this.modalContext?.reject()}>Cancel</uui-button>
                </div>
            </umb-body-layout>
        `;
    }

    static styles = unsafeCSS(styles);
}

declare global {
    interface HTMLElementTagNameMap {
        'umbraco-blog-icon-picker-dialog': IconPickerDialogElement;
    }
}
