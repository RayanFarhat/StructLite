import { writable } from 'svelte/store';
import { browser } from '$app/environment';
let localData
// TODO AccountData in cookie nit in local storage
if (browser) {
    localData = JSON.parse(window.localStorage.getItem("AccountData") as string) as AccountData;
}
export const accountDataStore = writable(localData ? localData : {
    username: "",
    isPro: false,
    proEndingDate: "",
    token: "",
    expiration: ""
})

if (browser) {
    accountDataStore.subscribe(v => window.localStorage.setItem('AccountData', JSON.stringify(v)));
}

export type AccountData = {
    username: string;
    isPro: boolean;
    proEndingDate: string;
    token: string;
    expiration: string;
};

export function setAccountData(state: AccountData) {
    accountDataStore.update((s) => state);
}

export function getAccountData(): AccountData {
    return JSON.parse(window.localStorage.getItem('AccountData') as string) as AccountData;
}

export function clearAccountData() {
    let state = {
        username: "",
        isPro: false,
        proEndingDate: "",
        token: "",
        expiration: ""
    };
    accountDataStore.update((s) => state);
}