# UnlockWorld

UnlockWorld is a Windows application that enables users to unlock disabled UI elements in other applications. With a simple drag-and-drop interface, users can identify and enable disabled controls across various Windows applications.

## Features

- Drag-and-drop tool to identify UI elements in other applications
- Automatic detection of disabled UI elements
- Single-click enablement of disabled controls
- Support for most Windows applications (WinForms, WPF, Win32, etc.)
- Simple and intuitive user interface

## Installation

### Prerequisites
- Windows operating system
- .NET Framework 4.7.2 or higher
- Administrator privileges (for accessing certain applications)

### Setup
1. Download the latest release from the [Releases](https://github.com/yourusername/UnlockWorld/releases) page
2. Run the installer and follow the on-screen instructions
3. Launch UnlockWorld from the Start menu

## Usage

1. Launch UnlockWorld
2. Click and hold the "Finder" tool button
3. Drag the tool to the disabled UI element you want to enable
4. Release the mouse button
5. The element should now be enabled if possible

## How It Works

UnlockWorld uses the Windows UI Automation framework to interact with UI elements across different applications. It identifies disabled controls and attempts to enable them using appropriate automation patterns.

## Contributing

Contributions are welcome! Please see the [Contributing Guide](CONTRIBUTING.md) for more information.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Microsoft for the UI Automation framework
- Contributors and testers who have helped improve this tool
