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
                src: ['_Static/JS/dist/<%= pkg.name %>.js', '_Static/Css/<%= pkg.name %>.css']
            }
        },
        filerev_replace: {
            options: {
                assets_root: './'
            },
            compiled_assets: {
                src: ['_Static/JS/dist/<%= pkg.name %>.js', '_Static/Css/<%= pkg.name %>.css']
            },
            views: {
                options: {
                    views_root: './'
                },
                src: 'Default.aspx'
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
                }
            }
        },
    });

    grunt.loadNpmTasks('grunt-contrib-sass');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-filerev');
    grunt.loadNpmTasks('grunt-filerev-replace');
    grunt.registerTask('default', ['sass']);
    grunt.registerTask('default', ['sass', 'concat', 'uglify', 'filerev', 'filerev_replace']);
    grunt.registerTask('prod-release', ['sass', 'concat', 'uglify', 'filerev', 'filerev_replace']);

};