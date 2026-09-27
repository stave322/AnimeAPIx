# QR Code Creator Application

A Windows-based GUI application for creating QR codes with personal information.

## Features

- **Input Fields**: Enter Name, Course, Year, and optional Email
- **Picture Support**: Select and preview pictures before generating QR codes
- **QR Code Generation**: Automatically generates QR codes from your information
- **Save QR Codes**: Export QR codes as PNG or JPG images
- **API Integration**: Optional API server to store QR code data
- **Local Storage**: Saves metadata and QR codes locally

## Installation

### Step 1: Install Required Packages

Open PowerShell and run:

```powershell
cd "c:\Users\user\Desktop\Programming"
pip install -r requirements_qr.txt
```

If you want to use the API server, also install Flask:

```powershell
pip install flask flask-cors
```

## Usage

### Running the GUI Application

Open PowerShell and run:

```powershell
cd "c:\Users\user\Desktop\Programming"
python QR.py
```

### How to Use

1. **Fill in Information**
   - Enter your Name
   - Enter your Course
   - Enter your Year
   - Optionally enter your Email

2. **Add a Picture**
   - Click "Select Picture" button
   - Choose an image file (JPG, PNG, BMP)
   - Image preview will appear

3. **Generate QR Code**
   - Click "Generate QR Code" button
   - The QR code will appear on the right side
   - Information will be displayed below

4. **Save QR Code**
   - Click "Save QR Code" button
   - Choose location and filename
   - QR code will be saved as PNG or JPG

5. **Clear Data**
   - Click "Clear All" button to reset the form

### Optional: Using the API Server

#### Step 1: Start the API Server

Open a new PowerShell window and run:

```powershell
cd "c:\Users\user\Desktop\Programming"
python qr_api_server.py
```

You'll see:
```
Data directory: C:\Users\user\QRCode_API_Data
Starting QR Code API Server...
 * Running on http://localhost:5000
```

#### Step 2: Use API Features in the App

- Click "Save to Server (API)" button to save data to the API
- Data will be stored locally and can be retrieved via API endpoints

#### API Endpoints

- `POST /api/qrcode` - Save a new QR code
  ```json
  {
    "name": "John Doe",
    "course": "Computer Science",
    "year": "2024",
    "email": "john@example.com"
  }
  ```

- `GET /api/qrcodes` - Get all QR codes

- `GET /api/qrcode/<filename>` - Get specific QR code

- `GET /api/health` - Check server status

## File Structure

```
QR.py                    - Main GUI application
qr_api_server.py        - Optional API server
requirements_qr.txt     - Python dependencies
README.md               - This file
```

## Data Storage

Local data is stored in:
```
C:\Users\<YourUsername>\QRCode_Data\
```

Each QR code creates:
- `metadata.json` - Information about the QR code
- `qr_YYYYMMDD_HHMMSS.png` - Generated QR code image

## Troubleshooting

### "No module named qrcode"
Install packages: `pip install -r requirements_qr.txt`

### API Connection Error
Make sure the API server is running in another PowerShell window

### Image won't load
Make sure the image file is in JPG, PNG, or BMP format

## Technical Details

- **GUI Framework**: tkinter (built-in Python)
- **QR Generation**: qrcode library
- **Image Handling**: Pillow (PIL)
- **API Framework**: Flask (optional)

## Future Enhancements

- Database integration (SQLite/MySQL)
- Cloud storage (AWS S3/Google Drive)
- QR code scanning
- Multiple QR codes in one file
- Custom QR code styling
- Batch QR code generation

---

**Created**: February 2024
**Version**: 1.0
