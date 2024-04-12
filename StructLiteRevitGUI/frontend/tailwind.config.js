/** @type {import('tailwindcss').Config} */
export default {
  content: ['./src/**/*.{html,js,svelte,ts}'],
    theme: {
    extend: {},
  },
  plugins: [require("daisyui")],
  daisyui: {
    themes: [
      {
        mytheme: {
        
          "primary": "#dc2626",
                  
          "secondary": "#991b1b",
                  
          "accent": "#be123c",
                  
          "neutral": "#2a323c",
                  
          "base-100": "#1d232a",
                  
          "info": "#3abff8",
                  
          "success": "#36d399",
                  
          "warning": "#fbbd23",
                  
          "error": "#f87272",
        },
      },
    ],
  },
}

