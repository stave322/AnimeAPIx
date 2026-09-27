from flask import Flask, request, jsonify
from flask_cors import CORS
import json
import os
from datetime import datetime
import base64
from io import BytesIO
from PIL import Image

app = Flask(__name__)
CORS(app)

# Directory to store QR codes and metadata
DATA_DIR = os.path.join(os.path.expanduser("~"), "QRCode_API_Data")
os.makedirs(DATA_DIR, exist_ok=True)

@app.route('/api/qrcode', methods=['POST'])
def save_qrcode():
    """Save QR code data to server"""
    try:
        data = request.json
        
        # Validate required fields
        required_fields = ['name', 'course', 'year']
        if not all(field in data for field in required_fields):
            return jsonify({'error': 'Missing required fields'}), 400
        
        # Create filename
        filename = f"{data['name'].replace(' ', '_')}_{datetime.now().strftime('%Y%m%d_%H%M%S')}"
        
        # Save metadata
        metadata = {
            'name': data.get('name'),
            'course': data.get('course'),
            'year': data.get('year'),
            'email': data.get('email', ''),
            'timestamp': datetime.now().isoformat()
        }
        
        metadata_path = os.path.join(DATA_DIR, f"{filename}.json")
        with open(metadata_path, 'w') as f:
            json.dump(metadata, f, indent=2)
        
        # Handle QR code image if provided
        if 'qr_image' in data:
            try:
                # Decode base64 image
                image_data = base64.b64decode(data['qr_image'])
                image_path = os.path.join(DATA_DIR, f"{filename}.png")
                with open(image_path, 'wb') as f:
                    f.write(image_data)
            except Exception as e:
                print(f"Error saving image: {e}")
        
        return jsonify({
            'success': True,
            'message': 'QR code saved successfully',
            'filename': filename,
            'id': filename
        }), 201
    
    except Exception as e:
        return jsonify({'error': str(e)}), 500

@app.route('/api/qrcodes', methods=['GET'])
def get_qrcodes():
    """Get all saved QR codes"""
    try:
        qrcodes = []
        for file in os.listdir(DATA_DIR):
            if file.endswith('.json'):
                with open(os.path.join(DATA_DIR, file), 'r') as f:
                    qrcodes.append(json.load(f))
        
        return jsonify({'qrcodes': qrcodes}), 200
    except Exception as e:
        return jsonify({'error': str(e)}), 500

@app.route('/api/qrcode/<filename>', methods=['GET'])
def get_qrcode(filename):
    """Get specific QR code"""
    try:
        metadata_path = os.path.join(DATA_DIR, f"{filename}.json")
        image_path = os.path.join(DATA_DIR, f"{filename}.png")
        
        if not os.path.exists(metadata_path):
            return jsonify({'error': 'QR code not found'}), 404
        
        with open(metadata_path, 'r') as f:
            data = json.load(f)
        
        return jsonify(data), 200
    except Exception as e:
        return jsonify({'error': str(e)}), 500

@app.route('/api/health', methods=['GET'])
def health_check():
    """Health check endpoint"""
    return jsonify({'status': 'ok'}), 200

if __name__ == '__main__':
    print(f"Data directory: {DATA_DIR}")
    print("Starting QR Code API Server...")
    app.run(debug=True, host='localhost', port=5000)
