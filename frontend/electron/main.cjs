const { app, BrowserWindow } = require('electron');
const path = require('path');
const { spawn } = require('child_process');

let mainWindow;
let apiProcess;

const API_FILENAME = process.platform === 'win32' ? 'TaskFlow.API.exe' : 'TaskFlow.API';

function createWindow() {
    mainWindow = new BrowserWindow({
        width: 1280,
        height: 800,
        webPreferences: {
            nodeIntegration: false,
            contextIsolation: true
        },
        autoHideMenuBar: true,
        icon: path.join(__dirname, '../public/favicon.ico')
    });

    if (process.env.NODE_ENV === 'development') {
        mainWindow.loadURL('http://localhost:5173');
        // mainWindow.webContents.openDevTools();
    } else {
        mainWindow.loadFile(path.join(__dirname, '../dist/index.html'));
    }
}

function startApi() {
    let apiPath;

    if (process.env.NODE_ENV === 'development') {
        // In dev, we assume user runs backend manually via 'dotnet run'
        console.log('Development mode: Backend should be running manually.');
        return;
    } else {
        // In production, API is in 'resources/backend'
        apiPath = path.join(process.resourcesPath, 'backend', API_FILENAME);
    }

    console.log('Starting API from:', apiPath);

    apiProcess = spawn(apiPath, [], {
        cwd: path.dirname(apiPath),
        windowsHide: true,
        env: { ...process.env, ASPNETCORE_URLS: 'http://localhost:5000' }
    });

    apiProcess.on('error', (err) => {
        console.error('Failed to start API:', err);
    });

    apiProcess.on('close', (code) => {
        console.log(`API process exited with code ${code}`);
    });
}

function killApi() {
    if (apiProcess) {
        console.log('Killing API process...');
        apiProcess.kill();
        apiProcess = null;
    }
}

app.whenReady().then(() => {
    startApi();
    createWindow();

    app.on('activate', () => {
        if (BrowserWindow.getAllWindows().length === 0) createWindow();
    });
});

app.on('window-all-closed', () => {
    killApi();
    if (process.platform !== 'darwin') app.quit();
});

app.on('before-quit', () => {
    killApi();
});
