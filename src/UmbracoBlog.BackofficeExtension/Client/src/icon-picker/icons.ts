import { IconName, IconPrefix, library } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { far } from '@fortawesome/free-regular-svg-icons';
import { fab } from '@fortawesome/free-brands-svg-icons';

let libraryLoaded = false;
export let loadIconLibrary = () => {
    if(libraryLoaded) {
        return;
    }

    library.add(fas, far, fab);
    libraryLoaded = true;
};

const mappedIcons = [...Object.values(fas), ...Object.values(far), ...Object.values(fab)]
    .map(icon => {
        const iconName = icon.iconName.toLowerCase();
        const alternativeNames = icon.icon[2]
            .filter((name: string) => typeof name === 'string')
            .map((name: string) => name.toLowerCase());

        return {
            ...icon,
            searchableNames: [
                iconName,
                ...alternativeNames
            ]
        }
    })
    .sort((a, b) => a.iconName.localeCompare(b.iconName));

const uniqueIcons = mappedIcons.filter((icon, index, self) =>
    index === self.findIndex((i) => i.prefix === icon.prefix && i.iconName === icon.iconName)
);

export const allIcons = uniqueIcons;

export interface Icon {
    prefix: IconPrefix;
    iconName: IconName;
    searchableNames: string[];
}
