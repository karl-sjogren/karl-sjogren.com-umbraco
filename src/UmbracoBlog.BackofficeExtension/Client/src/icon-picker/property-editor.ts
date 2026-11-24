import { css, customElement, html, nothing, property, state, unsafeCSS, unsafeSVG } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";
import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";
import { type UmbPropertyEditorUiElement } from "@umbraco-cms/backoffice/property-editor";
import { UMB_MODAL_MANAGER_CONTEXT, type UmbModalManagerContext } from "@umbraco-cms/backoffice/modal";
import { ICON_PICKER_DIALOG_TOKEN } from "./dialog.token.ts";
import { findIconDefinition, IconName, IconPrefix, icon as svgIcon } from '@fortawesome/fontawesome-svg-core';
import { loadIconLibrary } from "./icons.ts";

import styles from './property-editor.scss?inline';

@customElement("umbraco-blog-icon-picker-property-editor")
export default class IconPickerPropertyEditorElement extends UmbLitElement implements UmbPropertyEditorUiElement {
    @property({ type: String }) value = '';
    @property({ type: Boolean, reflect: true }) readonly = false;
    @property({ type: Boolean }) mandatory = false;

    @state() private _modalManager?: UmbModalManagerContext;

    constructor() {
        super();
        this.consumeContext(UMB_MODAL_MANAGER_CONTEXT, (context) => (this._modalManager = context));
    }

    connectedCallback(): void {
        super.connectedCallback();
        loadIconLibrary();
    }

    async #change() {
        const prefix = this.value?.split("|")[0];
        const iconName = this.value?.split("|")[1];

        const modal = this._modalManager?.open(this, ICON_PICKER_DIALOG_TOKEN, {
            data: { prefix, iconName },
        });

        if (!modal) {
            return;
        }

        try {
            const selected = await modal.onSubmit();
            console.log('Selected icon:', selected);

            const iconString = selected.prefix + "|" + selected.iconName;

            if (typeof selected === "object" && selected !== null) {
                this.value = iconString;
                this.dispatchEvent(new UmbChangeEvent());
            }
        } catch {
            console.log('Modal was closed without selecting an icon');
        }
    }

    #preview() {
        const prefix = this.value?.split("|")[0];
        const iconName = this.value?.split("|")[1];
        if (!prefix || !iconName) {
            return html`<div class="icon-preview">No icon selected</div>`;
        }

        const lookup = findIconDefinition({ prefix: prefix as IconPrefix, iconName: iconName as IconName });
        const svg = svgIcon(lookup);
        const htmlString = [...svg.node].map(node => node.outerHTML).join('');
        return html`<div class="icon-preview">${unsafeSVG(htmlString)}</div>`;
    }

    #clear(event?: Event) {
        event?.stopPropagation();
        this.value = "";
        this.dispatchEvent(new UmbChangeEvent());
    }

    #iconName() {
        const iconName = this.value?.split("|")[1];

        return !!iconName
            ? iconName
            : "Selected icon";
    }

    #iconType() {
        const prefix = this.value?.split("|")[0];
        if (!prefix) {
            return "";
        }

        return prefix === "fas" ? "Solid" : prefix === "far" ? "Regular" : prefix === "fab" ? "Brands" : "";
    }

    render() {
        return html`
            ${this.value
                ? html`
                    <uui-ref-list>
                        <uui-ref-node
                            name=${this.#iconName()}
                            detail=${this.#iconType()}
                            standalone
                            class="selected-icon"
                            ?readonly=${this.readonly}
                            @open=${this.#change}>
                            <span class="icon-preview" slot="icon" aria-hidden="true">${this.#preview()}</span>
                            ${this.readonly
                                ? nothing
                                : html`
                                    <uui-action-bar slot="actions">
                                        <uui-button label="Remove selected icon" @click=${this.#clear}>Remove</uui-button>
                                    </uui-action-bar>
                                `}
                        </uui-ref-node>
                    </uui-ref-list>
                `
                : this.readonly
                    ? html`<span class="empty">No icon selected</span>`
                    : html`<uui-button id="choose-icon" look="placeholder" label="Choose icon" @click=${this.#change}>Choose</uui-button>`}
    `;
    }

    static styles = unsafeCSS(styles);
}

declare global {
    interface HTMLElementTagNameMap {
        'umbraco-blog-icon-picker-property-editor': IconPickerPropertyEditorElement;
    }
}
