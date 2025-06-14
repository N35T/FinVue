const fs = require('fs');
const path = require('path');

// Get the value from environment variable or fallback
const apiBaseUrl = process.env.API_BASE_URL || 'http://localhost:4200/api';
const identityUrl = process.env.IDENTITY_PROVIDER_URL || 'https://identity.n35t.local/auth/login?returnUrl=http://localhost:4200'

// Define the directory and file path
const dirPath = path.join(__dirname, 'app', 'environment');
const filePath = path.join(dirPath, 'environment.ts');

// Define the content
const fileContent = `export const environment = {
    API_BASE_URL: '${apiBaseUrl}',
    IDENTITY_PROVIDER_URL: '${identityUrl}'
};
`;

// Ensure directory exists
fs.mkdirSync(dirPath, { recursive: true });

// Write the file
fs.writeFileSync(filePath, fileContent, 'utf8');

console.log(`environment.ts created!`);