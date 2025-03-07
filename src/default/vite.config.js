import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from "@tailwindcss/vite";

/** @type {import('vite').UserConfig} */
export default defineConfig({
    plugins: [react({ jsxRuntime: 'classic'}),tailwindcss()], // jsxRuntime: 'classic' is required for fast-refresh for .js files
    root: "./src/SAFEr.App.Client",
    server: {
        port: 8080,
        proxy: {
            '/api': 'http://localhost:5000',
        },
        watch: {
            ignored: [
                "**/*.fs" // Don't watch F# files
            ]
        }
    },
    build: {
        outDir:"../../publish/app/public"
    }
})
