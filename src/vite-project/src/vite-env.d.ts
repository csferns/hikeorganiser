/// <reference types="vite/client" />

interface ImportMetaEnv {
    readonly VITE_REACT_APP_API_ENDPOINT: string
    readonly VITE_GOOGLE_MAPS_API_KEY: string
    readonly VITE_PORT: number
}

interface ImportMeta {
    readonly env: ImportMetaEnv
}