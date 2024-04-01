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
                "Subscription are valid from the date of purchase until the end of time A valid and active License is required in order to access updates and downloads for purchased product, We are referring to the license as the Pro subscription.",
        },
        {
            title: "Payment and Fees",
            paragraph:
                "Some parts of Service are billed on a subscription basis (“Subscription(s)”). You will be billed in advance on a recurring and periodic basis (“Billing Cycle”). Billing cycles are set either on a monthly or yearly, depending on the type of subscription plan you select when purchasing a Subscription. To proceed with your subscription payment, a valid payment method, such as PayPal, is necessary. You agree to provide Ryvarr with precise and comprehensive billing details, encompassing full name, address, state, zip code, telephone number, and accurate payment method information required by PayPal. Please note that any issues encountered with PayPal preventing payment are beyond our responsibility.",
        },
        {
            title: "Transferring Licenses",
            paragraph: "Licenses are non-transferable",
        },
        {
            title: "License Use and Abuse",
            paragraph:
                "We reserve the right to limit or revoke licenses at any time in cases of abuse, such as re-selling access to your account or distributing your account on the open internet. Licenses that are revoked are not eligible for refunds.",
        },
        {
            title: "Software Updates",
            paragraph:
                "If you've subscribed to the pro plan, updates are automatically provided at no additional cost for the duration of your active account. There's no extra charge for updates, ensuring a seamless experience for you or any user.",
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
            title: "Refunds",
            paragraph:
                "We acknowledge that our product might not align with everyone's requirements. That's why we provide guided tours before purchase, ensuring clarity about its suitability for you. As our subscription can be to one month, refunds aren't offered due to the short duration and low price. We will only consider issuing a refund if we have ceased the website and server, discontinued the product, or paused it for several days.",
        },
        {
            title: "Website Functionality",
            paragraph:
                "This website is optimized for most modern browsers, with primary testing conducted on Chrome. If you encounter any issues, we recommend accessing the website using the Chrome browser for optimal functionality.",
        },
        {
            title: "Fee Changes",
            paragraph:
                "You agree that the subscription fee may be subject to change in the future due to tax adjustments, third-party modifications, or any reasons deemed necessary for our business to make decisions regarding subscription price alterations.",
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
