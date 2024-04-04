import { browser } from "$app/environment";

export function gotoURL(url: string) {
    if (browser) {
        window.location.href = url;
    }
}