import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';
import viteTsconfigPaths from 'vite-tsconfig-paths';

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

const certificateName = "ga360.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
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

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7030';

export default defineConfig({
    plugins: [plugin(), viteTsconfigPaths()],
    define: {
        global: 'window'
    },
    resolve: {
        alias: [
            //{
            //  find: /^~(.+)/,
            //  replacement: path.join(process.cwd(), 'node_modules/$1')
            //},
            //{
            //  find: /^src(.+)/,
            //  replacement: path.join(process.cwd(), 'src/$1')
            //},
          ]
    },
    server: {
        proxy: {
            '^/weatherforecast': {
                target: 'https://localhost:7030/',
                secure: false
            },
            '^/menu': {
                target: 'https://localhost:7030/',
                secure: false
            },
            '^/api/customer/list': {
                target,
                secure: false
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
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        }
    }
})

