import { UmbModalToken } from "@umbraco-cms/backoffice/modal";

export type IconPickerSettingsDialogData = {
    prefix: string;
    iconName: string;
}

export type IconPickerSettingsDialogValue = {
    prefix: string;
    iconName: string;
}

export const ICON_PICKER_DIALOG_TOKEN = new UmbModalToken<IconPickerSettingsDialogData, IconPickerSettingsDialogValue>('UmbracoBlog.IconPickerModal', {
    modal: {
        type: 'sidebar',
        size: 'medium'
    }
});
