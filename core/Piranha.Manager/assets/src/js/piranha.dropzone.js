// Prevent Dropzone from auto discoveringr all elements:
Dropzone.autoDiscover = false;

piranha.dropzone = new function () {
    var self = this;

    self.init = function (selector, options) {

        var defaultOptions = {
            paramName: 'Uploads',
            url: piranha.baseUrl + "manager/api/media/upload",
            headers: piranha.utils.antiForgeryHeaders(false),
            thumbnailWidth: 70,
            thumbnailHeight: 70,
            previewsContainer: selector + " .media-list",
            previewTemplate: document.querySelector("#media-upload-template").innerHTML,
            uploadMultiple: true,
            init: function () {
                var self = this;

                // Default addedfile callback
                if (!options.addedfile) {
                    options.addedfile = function (file) {
                        //Add logic to this method or explain why it's empty
                    }
                }

                // Default removedfile callback
                if (!options.removedfile) {
                    options.removedfile = function (file) {
                          // this function is empty now, but will be filled in the future!
                    }
                }

                // Default error callback
                if (!options.error) {
                    options.error = function (file) {
                        
                       // this function is empty now, but will be filled in the future!
                        
                    }
                }

                // Default complete callback
                if (!options.complete) {
                    options.complete = function (file) {
                        //console.log(file)
                        if (file.status !== "success" && file.xhr.responseText !== "") {
                            var response = JSON.parse(file.xhr.responseText);
                            file.previewElement.querySelector("[data-dz-errormessage]").innerText = response.body;
                        }
                    }
                }

                // Default queuecomplete callback
                if (!options.queuecomplete) {
                    options.queuecomplete = function () {
                        return undefined;
                    }
                }

                self.on("addedfile", options.addedfile);
                self.on("removedfile", options.removedfile);
                self.on("complete", options.complete);
                self.on("queuecomplete", options.queuecomplete);
            }
        };

        var config = Object.assign(defaultOptions, options);

        return new Dropzone(selector + " form", config);
    }
};
