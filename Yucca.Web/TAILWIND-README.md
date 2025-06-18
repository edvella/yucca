# Tailwind CSS Setup for Yucca Web

This project uses Tailwind CSS for styling. Follow these instructions to set it up properly.

## Prerequisites

- Node.js installed on your development machine
- NPM or Yarn package manager

## CI/CD Integration

Tailwind CSS building has been integrated into the GitHub Actions workflow. The `buildandtestsolution.yml` workflow installs Node.js dependencies and builds the Tailwind CSS before building the .NET solution.

## Installation

1. Navigate to the Yucca.Web directory:
```
cd d:\source\yucca\Yucca.Web
```

2. Install the dependencies:
```
npm install
```

3. Build the Tailwind CSS file:
```
npm run build:css
```

## Development

When developing, you can watch for changes and automatically rebuild the CSS:

```
npx tailwindcss -i ./wwwroot/css/app.css -o ./wwwroot/css/app.min.css --watch
```

## Icon Usage

The project includes Heroicons integrated as Blazor components. To use an icon, add the appropriate component:

```html
<MagnifyingGlassIcon CssClass="w-5 h-5 text-gray-500" />
```

Icon components are located in the `Components/Shared/Icons` directory.

## Troubleshooting

### CSS Not Building

If you're seeing issues with the Tailwind CSS not building:

1. Make sure Node.js is properly installed
2. Run `npm ci` instead of `npm install` to ensure exact package versions
3. Check that the input CSS file path is correct in the package.json scripts

### CI/CD Issues

If the GitHub Actions workflow is failing at the Tailwind CSS build step:

1. Check that the package-lock.json file is committed to the repository
2. Verify the paths in the workflow file match your project structure
3. Make sure the Node.js version in the workflow matches your local development environment
