export async function getData() {
    const res = await fetch("http://localhost:5555/Json", {
        method: "GET",
        mode: 'cors',
        headers: {
            Accept: 'application/json',
            "Content-Type": "application/json",
        },
    });
    const resJson = await res.json();
    return resJson;
}