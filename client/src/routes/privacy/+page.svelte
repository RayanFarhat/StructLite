<script lang="ts">
    let terms: Term[];
    type Term = {
        title: string;
        paragraph: string;
    };
    terms = [
        {
            title: "What personal data we collect and why we collect it",
            paragraph:
                "JWT tokens\nWhen you sign in to your account, we generate a JWT token containing your username, ID, and additional details. This token is stored in your browser, enabling seamless authorized access to our website without requiring you to sign in repeatedly. The token remains valid for one week; after this period, you will need to sign in again for continued access.\n\nYour email, username\nWe store your email and username to recognize you during sign-in and to ascertain your subscription and payment status for our products. We don't retain your password directly; instead, we store a hashed copy of it, ensuring that not even the company itself can view your password.",
        },
        {
            title: "How we use your information",
            paragraph:
                "We may use the information we collect from you when you register, make a purchase, sign up for our newsletter, or use certain other site features in the following ways:\n\tTo allow us to better service you in responding to your customer service requests.\n\tTo quickly process your transactions.\n\tTo send periodic emails regarding your order or other products and services.\n\tTo follow up with them after correspondence ( email or phone inquiries).\n\tTo personalize your experience and to allow us to deliver the type of content and product offerings in which you are most interested.",
        },
        {
            title: "How we protect your data",
            paragraph:
                "Your personal information is contained behind secured networks and is only accessible by a limited number of persons who have special access rights to such systems, and are required to keep the information confidential. In addition, all sensitive/credit information you supply is encrypted via Secure Socket Layer (SSL) technology.\n\nWe implement a variety of security measures when a user places an order enters, submits, or accesses their information to maintain the safety of your personal information with HTTPS.\n\nAll transactions are processed through a gateway provider and are not stored or processed on our servers.\n\nIf you are not comfortable with this security, don't register to our website and services.",
        },
        {
            title: "How long we retain your data",
            paragraph:
                "For users that register on our website (if any), we also store the personal information they provide in their user profile. All users can see, edit, or delete their personal information at any time (except they cannot change their username, email). Website administrators can also see and edit that information.",
        },
        {
            title: "What rights you have over your data",
            paragraph:
                "If you have an account on this site, you can request to receive an exported file of the personal data we hold about you, including any data you have provided to us. You can also request that we erase any personal data we hold about you. This does not include any data we are obliged to keep for administrative, legal, or security purposes.",
        },
        {
            title: "Where we send your data",
            paragraph:
                "If you make a purchase from us or submit our contact form, the data you provide or use during sign-in will be transmitted to our server host provider, who hosts our servers. We might occasionally send email communications to the address you provided. Rest assured, your data will never be transferred or sold to third parties.",
        },
        {
            title: "Contact information",
            paragraph:
                "You can contact us at any time via our contact form.: https://structlite.com/contact.",
        },
    ];
</script>

<br />
<br />
<br />
<br />
<div class="px-12">
    <h1 class="text-5xl lg:text-7xl">Privacy</h1>
    <br />
    <br />
    <br />
    {#each terms as term}
        <h2 class="text-3xl">{term.title}</h2>
        <br />
        <p style="white-space:  pre-wrap" class="text-lg">{term.paragraph}</p>
        <br />
        <br />
    {/each}
</div>
