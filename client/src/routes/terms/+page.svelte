<script lang="ts">
    let terms: Term[];
    type Term = {
        title: string;
        paragraph: string;
    };
    terms = [
        {
            title: "Licensing",
            paragraph:
                "The Software is intended to assist engineers trained, experienced, and qualified to perform structural analysis and design. Individuals without the proper training, experience, and qualifications shall not use the Software. While We work very hard to test and validate the Software, We cannot provide any guarantees it is free from errors. By using the StructLite Software, the User of the Software agrees that the User is ultimately responsible to validate the results of the Software.",
        },
        {
            title: "Agreement for Unexpected Events and Product Use",
            paragraph:
                "In the event of a server crash, database loss, or any unforeseen accidents, you('The User') agree that if any of these situations prevent you from using our product, you will not pursue legal action against us under any circumstances.",
        },
        {
            title: "Support Services",
            paragraph:
                "We aim to reply to all support requests. We are not expected to address every request; instead, we prioritize the most frequent and crucial ones, handling them as promptly as we can.\n you('The User') agree that you will not pursue legal action against us under any circumstances if we didn't response to your request.",
        },
        {
            title: "Website Functionality",
            paragraph:
                "This website is optimized for most modern browsers, with primary testing conducted on Chrome. If you encounter any issues, we recommend accessing the website using the Chrome browser for optimal functionality.",
        },
        {
            title: "Security",
            paragraph:
                "The Provider is not responsible for any information deleted from the system or maliciously hacked. Be sure to check who you add to the permissions to manage your system. Change your password as often as possible, and in any case every three months at least.",
        },
    ];
</script>

<br />
<br />
<br />
<br />
<div class="px-12">
    <h1 class="text-5xl lg:text-7xl">Terms of use & Agreements</h1>
    <br />
    <br />
    <br />
    {#each terms as term}
        <h2 class="text-3xl">{term.title}</h2>
        <br />
        <p class="text-lg">{term.paragraph}</p>
        <br />
        <br />
    {/each}
</div>
