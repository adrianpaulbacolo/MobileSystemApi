module.exports = function (grunt) {

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        concat: {
            options: {
                separator: ';\n',
                stripBanners: true
            },
            dist: {
                files: {
                    '_Static/JS/dist/concat-a.js': [
                        '_Static/JS/Mobile/jquery-1.11.3.min.js'
                        , '_Static/JS/Mobile/jquery.mobile-1.4.5.min.js'
                    ],
                    '_Static/JS/dist/concat-b.js': [
                        '_Static/JS/vendor/bootstrap.min.js'
                        , '_Static/JS/vendor/slick.min.js'
                        , '_Static/JS/vendor/lodash.js'
                        , '_Static/JS/vendor/amplify.min.js'
                    ],
                    '_Static/JS/dist/concat-c.js': [
                        '_Static/JS/modules/GPINT.js'
                        , '_Static/JS/modules/growl.js'
                        , '_Static/JS/modules/User.js'
                    ],
                    '_Static/Css/<%= pkg.name %>.css': [
                        '_Static/Css/base.css'
                        , '_Static/Css/styles.css'
                    ]
                },
            },
        },
        filerev: {
            dist: {
                src: [
                    '_Static/JS/dist/<%= pkg.name %>.js',
                    '_Static/JS/dist/<%= pkg.name %>.main.js',
                    '_Static/JS/dist/<%= pkg.name %>.login.js',
                    '_Static/JS/dist/<%= pkg.name %>.catalogue.js',
                    '_Static/Css/<%= pkg.name %>.css',
                    '_Static/Css/errors/styles.css',
                    '_Static/Css/errors/styles-light.css',
                    '_Static/JS/dist/<%= pkg.name %>.sw.min.js',
                    '_Static/JS/dist/<%= pkg.name %>.pointlevelinfo.min.js'
                ]
            }
        },
        filerev_replace: {
            options: {
                assets_root: './'
            },
            compiled_assets: {
                src: [
                    '_Static/JS/dist/<%= pkg.name %>.js',
                    '_Static/JS/dist/<%= pkg.name %>.main.js',
                    '_Static/JS/dist/<%= pkg.name %>.login.js',
                    '_Static/JS/dist/<%= pkg.name %>.catalogue.js',
                    '_Static/Css/<%= pkg.name %>.css',
                    '_Static/Css/errors/styles.css',
                    '_Static/Css/errors/styles-light.css',
                    '_Static/JS/dist/<%= pkg.name %>.sw.min.js',
                    '_Static/JS/dist/<%= pkg.name %>.pointlevelinfo.min.js'
                ]
            },
            views: {
                options: {
                    views_root: './'
                },
                src: [
                    '_Static/head.inc',
                    '_Secure/Login.aspx',
                    'Catalogue/Default.aspx',
                    '_Static/Pages/404.aspx',
                    '_Static/Pages/500.aspx',
                    '_Static/Pages/enhancement-all.aspx',
                    '_Static/Pages/enhancement.aspx',
                    'SpinWheel/Default.aspx'
                ]
            }
        },
        uglify: {
            options: {
                // the banner is inserted at the top of the output
                banner: '/*! <%= pkg.name %> <%= grunt.template.today("dd-mm-yyyy") %> */\n'
                , mangle: false
            },
            dist: {
                files: {
                    '_Static/JS/dist/<%= pkg.name %>.js': ['_Static/JS/dist/concat-*.js']
                    , '_Static/JS/dist/<%= pkg.name %>.main.js': ['_Static/JS/modules/Main.js']
                    , '_Static/JS/dist/<%= pkg.name %>.login.js': ['_Static/JS/modules/login.js']
                    , '_Static/JS/dist/<%= pkg.name %>.catalogue.js': ['_Static/catalogue/catalogue.js']
                    , '_Static/JS/dist/<%= pkg.name %>.sw.min.js': ['_Static/JS/modules/spinwheel.js']
                    , '_Static/JS/dist/<%= pkg.name %>.pointlevelinfo.min.js': ['_Static/JS/modules/pointlevelinfo.js']
                }
            }
        },
        sass: {
            dist: {
                options: {
                    style: 'compressed',
                    compass: false
                },
                files: {
                    '_Static/Css/base.css': '_Static/Css/scss/base.scss'
                    , '_Static/Css/styles.css': '_Static/Css/scss/styles.scss'
                    , '_Secure/VIP/css/style.css': '_Secure/VIP/scss/style.scss'
                    , '_Static/Css/errors/styles.css': '_Static/Css/errors/scss/styles.scss'
                    , '_Static/Css/errors/styles-light.css': '_Static/Css/errors/scss/styles-light.scss'
                    , '_Static/fonts/icomoon/style.css': '_Static/fonts/icomoon/style.scss'
                }
            }
        },
    });

    grunt.loadNpmTasks('grunt-contrib-sass');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-filerev');
    grunt.loadNpmTasks('grunt-filerev-replace');
    grunt.registerTask('default', ['sass', 'concat', 'uglify', 'filerev', 'filerev_replace']);
};