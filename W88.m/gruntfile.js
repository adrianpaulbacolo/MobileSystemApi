module.exports = function(grunt) {
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        sass: {
            dist: {
                options: {
                    style: 'compressed',
                    compass: false
                },
                files: {
                    '_Static/fonts/icomoon/style.css': '_Static/fonts/icomoon/style.scss',
                    '_Static/fonts/ionicons/ionicons-icons.css': '_Static/fonts/ionicons/_ionicons-icons.scss',
                    '_Static/Css/style.css': '_Static/scss/style.scss'
                }
            }   
        }
    });

    grunt.loadNpmTasks('grunt-contrib-sass');
    grunt.registerTask('default', ['sass']);
};