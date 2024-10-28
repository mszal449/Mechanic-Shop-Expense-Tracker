module.exports = {
  semi: true, // Add semicolons at the end of statements
  singleQuote: true, // Use single quotes instead of double quotes
  printWidth: 80, // Limit line length to 80 characters
  tabWidth: 2, // Set indentation width to 2 spaces
  useTabs: false, // Use spaces instead of tabs
  trailingComma: 'es5', // Add trailing commas in objects, arrays, etc.
  bracketSpacing: true, // Print spaces between brackets in objects
  arrowParens: 'always', // Always include parentheses around arrow function arguments
  endOfLine: 'lf', // Use line-feed (LF) for line breaks
  plugins: [require('prettier-plugin-tailwindcss')], // Add Tailwind CSS plugin for sorting class names
};
