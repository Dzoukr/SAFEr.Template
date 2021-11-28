module.exports = {
    mode: 'jit',
    purge: [
        './src/**/*.html',
        './src/**/*.fs',
        '.fable-build/**/*.js',
    ],
    variants: {},
    plugins: [
        require('daisyui'),
    ]
}
