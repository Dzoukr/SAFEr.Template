import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

/** @type {import('vite').UserConfig} */
export default defineConfig({
    plugins: [react()],
    root: "./src/SAFEr.App.Client",
    server: {
        port: 8080,
        proxy: {
            '/api': 'http://localhost:5000',
        }
    },
    build: {
        outDir:"../../publish/app/public"
    }

})