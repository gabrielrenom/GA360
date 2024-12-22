import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';
import viteTsconfigPaths from 'vite-tsconfig-paths';

const baseFolder = env.APPDATA !== undefined && env.APPDATA !== ''
    ? `${env.APPDATA}/ASP.NET/https`
    : `${env.HOME}/.aspnet/https`;

const certificateName = "ga360.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (env.NODE_ENV !== 'production' && (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath))) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit', }).status) {
        throw new Error("Could not create certificate.");
    }
}

console.log("HAZEL GROVE", env.NODE_ENV)

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7030';

export default defineConfig({
    plugins: [plugin(), viteTsconfigPaths()],
    define: {
        global: 'window'
    },
    resolve: {
        alias: [
            // Custom alias if needed
        ]
    },
    server: {
        proxy: {

            '^/menu': {
                target: 'https://localhost:7030/',
                secure: false
            },
            '^/api/customer/list': {
                target,
                secure: false
            },
            '^/api/customer/user': {
                target,
                secure: false
            },
            '^/api/customer/create': {
                target,
                secure: false
            },
            '^/api/customer/delete': {
                target,
                secure: false
            },
            '^/api/customer/update': {
                target,
                secure: false
            },
            '^/api/customer/get': {
                target,
                secure: false
            },
            '^/api/customer/get/basic': {
                target,
                secure: false
            },
            '^/api/customer/get/full': {
                target,
                secure: false
            },
            '^/api/customer/updatewithdocuments': {
                target,
                secure: false
            },
            '^/api/customer/get/documents': {
                target,
                secure: false
            },
            '^/api/customer/customerwithcoursequalificationrecords': {
                target,
                secure: false
            },
            '^/api/customer/batchupload': {
                target,
                secure: false
            },
            '^/api/trainingcentre/list': {
                target,
                secure: false
            },
            '^/api/ethnicity/list': {
                target,
                secure: false
            },
            '^/api/skill/list': {
                target,
                secure: false
            },
            '^/api/country/list': {
                target,
                secure: false
            },
            '^/api/course': {
                target,
                secure: false,
                changeOrigin: true
            },
            '^/api/qualification': {
                target,
                secure: false,
                changeOrigin: true
            },
            '^/api/trainingcentre': {
                target,
                secure: false,
                changeOrigin: true
            },
            '^/api/customer/customerswithcoursequalificationrecords': {
                target,
                secure: false,
                changeOrigin: true
            },
            '^/api/certificate': {
                target,
                secure: false,
                changeOrigin: true
            },
            '^/api/dashboard': {
                target,
                secure: false,
                changeOrigin: true
            },
            '^/api/configuration/redirecturl': {
                target,
                secure: false,
            },
            '^/api/configuration/logouturl': {
                target,
                secure: false,
            },
            '^/api/configuration/sessionout': {
                target,
                secure: false,
            },
            '^/signin-oidc': {
                target,
                secure: false
            },
            '^/bff/login': {
                target,
                secure: false
            },
            '^/bff/user': {
                target,
                secure: false
            },
            '^/signout-callback-oidc': {
                target,
                secure: false
            },
        },
        port: 5173,
        https: env.NODE_ENV !== 'production' ? {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        } : undefined
    }
});
