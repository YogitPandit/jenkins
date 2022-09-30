'use strict';
app.controller('TurnATimeController', ['$scope', "WarehouseService", "$filter", "$http", "ngTableParams", '$modal', function ($scope, WarehouseService, $filter, $http, ngTableParams, $modal) {
 
    $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));
    
   //if (UserRole.role == 'HQ Master login' && UserRole.OrderMaster == 'True' || UserRole.role == 'WH Master login' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ HR lead' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Accounts Associate' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Accounts Executive' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Accounts Lead' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ CS Associate' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ CS EXECUTIVE' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ CS LEAD' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Growth Associate' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Growth Executive' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Growth Lead' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ HR associate' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ HR executive' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ IC Associate' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ IC Executive' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ IC Lead' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Marketing Associate' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Marketing Executive' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Marketing Lead' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ MIS Associate' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ MIS EXECUTIVE' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ MIS LEAD' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Ops Associate' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Ops Executive' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Ops Lead' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Purchase Associate' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Purchase Executive ' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Purchase Lead' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Sales Associate' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Sales Executive' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Sales Lead' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Sourcing Associate' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Sourcing Executive' && UserRole.OrderMaster == 'True' || UserRole.role == 'HQ Sourcing Lead' && UserRole.OrderMaster == 'True' || UserRole.role == 'WH Agents' && UserRole.OrderMaster == 'True' || UserRole.role == 'WH cash manager' && UserRole.OrderMaster == 'True' || UserRole.role == 'WH delivery planner' && UserRole.OrderMaster == 'True' || UserRole.role == 'WH Ops associate' && UserRole.OrderMaster == 'True' || UserRole.role == 'WH Purchase executive' && UserRole.OrderMaster == 'True' || UserRole.role == 'WH Super visor' && UserRole.OrderMaster == 'True' || UserRole.role == 'WH Service lead' && UserRole.OrderMaster == 'True' || UserRole.role == 'WH Sales lead' && UserRole.OrderMaster == 'True' || UserRole.role == 'WH sales executives' && UserRole.OrderMaster == 'True' || UserRole.role == 'WH sales associates' && UserRole.OrderMaster == 'True' && UserRole.OrderMaster == 'True' || UserRole.role == 'WH Service lead') {

        $scope.compid = $scope.UserRole.compid;
 
        
        $scope.getWarehosues = function () {
            WarehouseService.getwarehouse().then(function (results) {

                $scope.warehouse = results.data;
            }, function (error) {
            })
        };

        $(function () {
            $('input[name="daterange"]').daterangepicker({
                timePicker: true,
                timePickerIncrement: 5,
                timePicker12Hour: true,
                format: 'MM/DD/YYYY h:mm A',
            });
        });
        $scope.WarehouseId ="";
        $scope.GetData = function () {
           
            var f = $('input[name=daterangepicker_start]');
            var g = $('input[name=daterangepicker_end]');
            var start = f.val();
            var end = g.val();

            if (!$('#dat').val() &&  $scope.WarehouseId == "") {
                start = null;
                end = null;
                alert("Please select one parameter");
                return;
            }
            $scope.TatData = [];
            $scope.TatDataDemo = [];
            var url = serviceBase + "api/TurnATime?start=" + start + "&end=" + end + "&WarehouseId=" + $scope.WarehouseId;
            $http.get(url).success(function (response) {
                $scope.TatData = response;
                $scope.TatDataDemo = response;
                
                $scope.RtD = 0;
                $scope.RtDhr = 0;
                $scope.AvgRtDhr = 0;
                $scope.AvgRtDhrst = "";

                $scope.Issued = 0;
                $scope.Issuedhr = 0;
                $scope.AvgIssuedhr = 0;
                $scope.AvgIssuedhrst = "";

                $scope.Delivered = 0;
                $scope.Deliveredhr = 0;
                $scope.AvgDeliveredhr = 0;
                $scope.AvgDeliveredhrst = "";

         

                for (var i = 0; i < $scope.TatDataDemo.length; i++) {
                   
                    if ($scope.TatDataDemo[i].ReadytoDdiffhours>0) {
                        $scope.RtDhr = $scope.RtDhr + $scope.TatDataDemo[i].ReadytoDdiffhours;
                        //$scope.RtD++;
                        $scope.AvgRtDhrst = "To Dispatch";
                    }
                    if ($scope.TatDataDemo[i].ReadytoDelivereddiffhours>0) {
                        $scope.Issuedhr = $scope.Issuedhr + $scope.TatDataDemo[i].ReadytoDelivereddiffhours;
                        //$scope.Issued++;
                        $scope.AvgIssuedhrst = "Issued,Shipped";
                    }
                   
                    if ($scope.TatDataDemo[i].Deliverydiffhours>0) {
                        $scope.Deliveredhr = $scope.Deliveredhr + $scope.TatDataDemo[i].Deliverydiffhours;
                        //$scope.Delivered++;
                        $scope.AvgDeliveredhrst = "Delivered";
                    }
                    
                }
                try {

                    $scope.AvgRtDhr = $scope.RtDhr / $scope.TatDataDemo.length;
                    if (isNaN($scope.AvgRtDhr)) {
                        $scope.AvgRtDhr=0;
                    }

                    $scope.AvgIssuedhr = $scope.Issuedhr / $scope.TatDataDemo.length;
                    if (isNaN($scope.AvgIssuedhr)) {
                        $scope.AvgIssuedhr = 0;
                    }

                    $scope.AvgDeliveredhr = $scope.Deliveredhr / $scope.TatDataDemo.length;
                    if (isNaN($scope.AvgDeliveredhr)) {
                        $scope.AvgDeliveredhr = 0;
                    }
                
                }
                catch (err) {
                    aler(err.message);
                }




        $(function () {
             var chart = new CanvasJS.Chart("chartContainer111",
             {
               title: {
               text: "Bar Chart with Percent"
               },
              data: [
               {
                   type: "column",
               //indexLabel : "{y}",
               toolTipContent: "{y}hr",
               dataPoints: [
               { label: $scope.AvgRtDhrst, y: $scope.AvgRtDhr },
               { label: $scope.AvgIssuedhrst, y: $scope.AvgIssuedhr },
               { label: $scope.AvgDeliveredhrst, y: $scope.AvgDeliveredhr }
               
               ]
              }
              ]
              });
           chart.render();
          });
                $scope.callmethod();
            })
          .error(function (data) {
          })
        }
        $scope.callmethod = function () {
            var init;
            return $scope.stores = $scope.TatData,
                $scope.searchKeywords = "",
                $scope.filteredStores = [],
               $scope.row = "",
                $scope.select = function (page) {
                    var end, start; console.log("select"); console.log($scope.stores);
                    return start = (page - 1) * $scope.numPerPage, end = start + $scope.numPerPage, $scope.currentPageStores = $scope.filteredStores.slice(start, end)
                },
                $scope.onFilterChange = function () {
                    console.log("onFilterChange"); console.log($scope.stores);
                    return $scope.select(1), $scope.currentPage = 1, $scope.row = ""
                },
                $scope.onNumPerPageChange = function () {
                    console.log("onNumPerPageChange"); console.log($scope.stores);
                    return $scope.select(1), $scope.currentPage = 1
                },
                $scope.onOrderChange = function () {
                    console.log("onOrderChange"); console.log($scope.stores);
                    return $scope.select(1), $scope.currentPage = 1
                },
                $scope.search = function () {
                    console.log("search");
                    console.log($scope.stores);
                    console.log($scope.searchKeywords);
                    return $scope.filteredStores = $filter("filter")($scope.stores, $scope.searchKeywords), $scope.onFilterChange()
                },
                $scope.order = function (rowName) {
                    console.log("order"); console.log($scope.stores);
                    return $scope.row !== rowName ? ($scope.row = rowName, $scope.filteredStores = $filter("orderBy")($scope.stores, rowName), $scope.onOrderChange()) : void 0
                },
                $scope.numPerPageOpt = [100],
                $scope.numPerPage = $scope.numPerPageOpt[0],
                $scope.currentPage = 1,
                $scope.currentPageStores = [],
                (init = function () {
                    return $scope.search(), $scope.select($scope.currentPage)
                })
            ()
            $scope.getWarehosues1111();
        }


       
        //............................Exel export Method.....................//

        alasql.fn.myfmt = function (n) {
            return Number(n).toFixed(2);
        }
        $scope.exportData1 = function () {

            alasql('SELECT OrderId,Status,ReadytoDdiffhours,ReadytoDelivereddiffhours,Deliverydiffhours,CustomerName,Skcode,GrossAmount,SalesPerson INTO XLSX("TatData.xlsx",{headers:true}) FROM ?', [$scope.TatData]);
        }
        //............................Exel export Method.....................//
    //}
    //else {
    //    window.location = "#/404";
    //}
}]);


