import { writable } from 'svelte/store';
import { browser } from '$app/environment';

export let DataStore = writable(null)
export async function getData() {
    if (browser) {
        const res = await fetch("http://localhost:5555/Json", {
            method: "GET",
            mode: 'cors',
            headers: {
                Accept: 'application/json',
                "Content-Type": "application/json",
            },
        });
        const resJson = await res.json();
        DataStore.update((s) => resJson);
        //return resJson;
    }
}