console.log("API URL is ", configuration.apiUrl);

var demoApp = angular.module("demoApp", ["angularFileUpload"]);

///////////////////////////////////////////////////////////////
demoApp.controller("messageLogController", function ($scope) {
    $scope.messages = "";
});


///////////////////////////////////////////////////////////////
demoApp.controller("metadataController", function ($scope, $http) {
    $scope.dateFormat = "yyyy-MM-dd HH:mm:ss Z";
    $scope.metadataList = [];

    $scope.loadData = function () {
        $http({ method: "GET", url: configuration.apiUrl + "/metadata" }).success(function (data) {
            $scope.metadataList = data;
        });
    };
    
    $scope.removeMetadata = function (metadata) {
        console.log("Remove metadata ", metadata.Id);

        $http({ method: "DELETE", url: configuration.apiUrl + "/metadata/" + metadata.Id }).success(function () {
            $scope.loadData();
        });
    };

    $scope.downloadFile = function (metadata) {
        console.log("Download file ", metadata.Id);
        window.open(configuration.apiUrl + "/download/" + metadata.Id, "_self");
    };

    $scope.loadData();
});

///////////////////////////////////////////////////////////////
demoApp.controller("uploadController", ["$scope", "$upload", function ($scope, $upload) {
    $scope.files = [];
    $scope.uploading = false;

    $scope.openFileSelectDialog = function () {
        $("#FileSelectDialog").trigger("click");
    };

    $scope.onFileSelect = function ($files) {
        console.log("Adding files to uploads list", $files);
        $scope.files = $scope.files.concat($files);
    };

    $scope.removeFileFromUploadsList = function (file) {
        console.log("Removing file from uploads list", file.name);
        var index = $scope.files.indexOf(file);
        if (index >= 0)
        {
            $scope.files.splice(index, 1);
        }
    };

    $scope.uploadSelectedFiles = function() {

        $scope.uploading = true;

        for (var i = 0; i < $scope.files.length; i++) {
            var file = $scope.files[i];

            $scope.upload = $upload.upload({
                url: configuration.apiUrl + '/upload',
                method: 'POST',
                //data: { myObj: $scope.myModelObj },
                file: file,
            }).progress(function (evt) {
                file.uploadProgress = parseInt(100.0 * evt.loaded / evt.total);
                console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
            }).success(function (data, status, headers, config) {
                // file is uploaded successfully
                var jsonString = JSON.stringify(data);
                console.log(jsonString);
                

                var index = $scope.files.indexOf(file);
                $scope.files.splice(index, 1);

                // todo. refresh data in the screen
                $(document).trigger("LogMessageEvent");
            });
            //.error(...)
            //.then(success, error, progress); 
        }

    };
}]);

///////////////////////////////////////////////////////////////
$("#GetApiHelp").on("click", function () {
    console.log("Open API help ");
    window.open(configuration.apiUrl + "/Help", "_new");
});
