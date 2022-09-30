
var app = angular.module("app", ["ngRoute", "ngAnimate", "ngAutocomplete", 'angularjs-dropdown-multiselect', 'angular-loading-bar', 'ngTableExport', 'LocalStorageModule', "ui.bootstrap", 'daterangepicker', 'ngTable', 'angularFileUpload', "easypiechart", "mgo-angular-wizard", "textAngular", "app.ui.ctrls", "app.ui.directives", "app.ui.services", "app.controllers", "app.directives", "app.form.validation", "app.ui.form.ctrls", "app.ui.form.directives", "app.tables", "app.task", "app.localization", "app.chart.ctrls", "app.chart.directives", 'angularUtils.directives.dirPagination'])
.directive('autocomplete', ['autocomplete-keys', '$window', '$timeout', function (Keys, $window, $timeout) {
    var template =
    '<input type="text" class="autocomplete-input" placeholder="{{placeHolder}}"' +
        'ng-class="inputClass"' +
        'ng-model="searchTerm"' +
        'ng-keydown="keyDown($event)"' +
        'ng-keypress="keyPress($event)"' +
        'ng-blur="onBlur()"' +
        'ng-readonly="ngReadonly" />' +

    '<div class="autocomplete-options-container">' +
        '<div class="autocomplete-options-dropdown" ng-if="showOptions">' +
            '<div class="autocomplete-option" ng-if="!hasMatches">' +
                '<span>No matches</span>' +
            '</div>' +

            '<ul class="autocomplete-options-list">' +
                '<li class="autocomplete-option" ng-class="{selected: isOptionSelected(option)}" ' +
                    'ng-style="{width: optionWidth}"' +
                    'ng-repeat="option in matchingOptions"' +
                    'ng-mouseenter="onOptionHover(option)"' +
                    'ng-mousedown="selectOption(option)"' +
                    'ng-if="!noMatches">' +
                    '<span>{{option[displayProperty]}}</span>' +
                '</li>' +
            '</ul>' +
        '</div>' +
    '</div>';
    return {
        template: template,
        restrict: 'E',
        scope: {
            searchTerm: '=?ngModel',
            options: '=',
            onSelect: '=',
            ngReadonly: '=',
            displayProperty: '@',
            inputClass: '@',
            clearInput: '@',
            placeHolder: '@'
        },
        controller: function ($scope) {
            $scope.highlightedOption = null;
            $scope.showOptions = false;
            $scope.matchingOptions = [];
            $scope.hasMatches = false;
            $scope.selectedOption = null;

            $scope.isOptionSelected = function (option) {
                return option === $scope.highlightedOption;
            };

            $scope.processSearchTerm = function (term) {
                // console.log('ch-ch-ch-changin');
                if (term.length > 0) {
                    if ($scope.selectedOption) {
                        if (term != $scope.selectedOption[$scope.displayProperty]) {
                            $scope.selectedOption = null;
                        } else {
                            $scope.closeAndClear();
                            return;
                        }
                    }

                    var matchingOptions = $scope.findMatchingOptions(term);
                    $scope.matchingOptions = matchingOptions;
                    if (!$scope.matchingOptions.indexOf($scope.highlightedOption) != -1) {
                        $scope.clearHighlight();
                    }
                    $scope.hasMatches = matchingOptions.length > 0;
                    $scope.showOptions = true;
                    $scope.setOptionWidth();
                } else {
                    $scope.closeAndClear();
                }
            };

            $scope.findMatchingOptions = function (term) {
                if (!$scope.options) {
                    throw 'You must define a list of options for the autocomplete ' +
                    'or it took too long to load';
                }
                return $scope.options.filter(function (option) {
                    var searchProperty = option[$scope.displayProperty];
                    if (searchProperty) {
                        var lowerCaseOption = searchProperty.toLowerCase();
                        var lowerCaseTerm = term.toLowerCase();
                        return lowerCaseOption.indexOf(lowerCaseTerm) != -1;
                    }
                    return false;
                });
            };

            $scope.findExactMatchingOptions = function (term) {
                return $scope.options.filter(function (option) {
                    var lowerCaseOption = option[$scope.displayProperty].toLowerCase();
                    var lowerCaseTerm = term ? term.toLowerCase() : '';
                    return lowerCaseOption == lowerCaseTerm;
                });
            };

            $scope.keyDown = function (e) {
                switch (e.which) {
                    case Keys.upArrow:
                        e.preventDefault();
                        if ($scope.showOptions) {
                            $scope.highlightPrevious();
                        }
                        break;
                    case Keys.downArrow:
                        e.preventDefault();
                        if ($scope.showOptions) {
                            $scope.highlightNext();
                        } else {
                            $scope.showOptions = true;
                            if ($scope.selectedOption) {
                                $scope.highlightedOption = $scope.selectedOption;
                            }
                        }
                        break;
                    case Keys.enter:
                        e.preventDefault();
                    case Keys.tab:
                        if ($scope.highlightedOption) {
                            $scope.selectOption($scope.highlightedOption);
                        } else {
                            var exactMatches = $scope.findExactMatchingOptions($scope.searchTerm);
                            if (exactMatches[0]) {
                                $scope.selectOption(exactMatches[0]);
                            }
                        }
                        break;
                    case Keys.escape:
                        $scope.closeAndClear();
                        break;
                }
            };

            $scope.keyPress = function (e) {
                switch (e.which) {
                    case Keys.upArrow:
                    case Keys.downArrow:
                    case Keys.enter:
                    case Keys.escape:
                        break;
                    default:
                        $timeout(function () { $scope.processSearchTerm($scope.searchTerm); });
                        break;
                }
            };

            //$scope.$watch('searchTerm', function(term, oldTerm) {
            //    if (term && term !== oldTerm) {
            //        $scope.processSearchTerm(term);
            //    }
            //});

            $scope.highlightNext = function () {
                if (!$scope.highlightedOption) {
                    $scope.highlightedOption = $scope.matchingOptions[0];
                } else {
                    var currentIndex = $scope.getCurrentOptionIndex();
                    var nextIndex = currentIndex + 1 == $scope.matchingOptions.length
                        ? 0 : currentIndex + 1;
                    $scope.highlightedOption = $scope.matchingOptions[nextIndex];
                }
            };

            $scope.highlightPrevious = function () {
                if (!$scope.highlightedOption) {
                    $scope.highlightedOption = $scope.matchingOptions[$scope.matchingOptions.length - 1];
                } else {
                    var currentIndex = $scope.getCurrentOptionIndex();
                    var previousIndex = currentIndex == 0
                        ? $scope.matchingOptions.length - 1
                        : currentIndex - 1;
                    $scope.highlightedOption = $scope.matchingOptions[previousIndex];
                }
            };

            $scope.onOptionHover = function (option) {
                $scope.highlightedOption = option;
            };

            $scope.$on('app:clearInput', function () {
                $scope.searchTerm = '';
            });

            $scope.clearHighlight = function () {
                $scope.highlightedOption = null;
            };

            $scope.closeAndClear = function () {
                $scope.showOptions = false;
                $scope.clearHighlight();
            };

            $scope.selectOption = function (option) {
                // console.log('selected the option');
                $scope.selectedOption = option;
                $scope.onSelect(option);

                if ($scope.clearInput != 'False' && $scope.clearInput != 'false') {
                    $scope.searchTerm = '';
                } else {
                    $scope.searchTerm = option[$scope.displayProperty];
                }
                $scope.closeAndClear();
            };

            $scope.onBlur = function () {
                $scope.closeAndClear();
            };

            $scope.getCurrentOptionIndex = function () {
                return $scope.matchingOptions.indexOf($scope.highlightedOption);
            };
        },
        link: function (scope, elem, attrs) {
            scope.optionWidth = '400px';
            var inputElement = elem.children('.autocomplete-input')[0];

            scope.setOptionWidth = function () {
                // console.log(inputElement.offsetWidth);
                $timeout(function () {
                    var pixelWidth = inputElement.offsetWidth > 400 ? 400 : inputElement.offsetWidth - 2;
                    scope.optionWidth = pixelWidth + 'px';
                });
            };

            angular.element(document).ready(function () {
                scope.setOptionWidth();
            });

            angular.element($window).bind('resize', function () {
                scope.setOptionWidth();
            });
        }
    };
}])
.factory('autocomplete-keys', function () {
    return {
        upArrow: 38,
        downArrow: 40,
        enter: 13,
        escape: 27,
        tab: 9
    };
});
app.config(["$routeProvider", function ($routeProvider) {
    return $routeProvider.when("/", {
        redirectTo: "/DashboardReport"
    })
        .when("/ProviderLink", {
            controller: "ProviderLinkCrtl",
            templateUrl: "/views/ProviderLink/ProviderLink.html"
        })

    	.when("/dashboard", {
    	    controller: "dashboardController",
    	    templateUrl: "/views/dashboard.html"
    	})
        .when("/CpMatrix", {
            controller: "CiMatrixController",
            templateUrl: "/views/Order/CiMatrix.html"
        })
        .when("/Document", {
            controller: "DocumentController",
            templateUrl: "/views/admin/Document.html"
        })
        .when("/ChangePassword", {
            controller: "ChangePassword",
            templateUrl: "/views/pages/ChangePassword.html"
        })
        .when("/TurnATime", {
            controller: "TurnATimeController",
            templateUrl: "views/Delivery/TurnATime.html"
        })
        .when("/Agent", {
            controller: "AgentsController",
            templateUrl: "/views/admin/Agent.html"
        })
        .when("/VisitReport", {
            controller: "NotVisitCtrl",
            templateUrl: "views/Customer/NotVisit.html"
        })
        .when("/DialValuePoint", {
            controller: "DialValuePointController",
            templateUrl: "/views/Pramotion/DialValuePoint.html"
        })
        .when("/DialPoint", {
            controller: "DialPointController",
            templateUrl: "/views/Pramotion/DialPoint.html"
        })
        .when("/brandwisepramotion", {
            controller: "BrandwisepramotionController",
            templateUrl: "/views/Pramotion/Brandwisepramotion.html"
        })
        .when("/Exclusivebrand", {
            controller: "ExclusiveBrandController",
            templateUrl: "/views/Pramotion/ExclusiveBrand.html"
        })
        .when("/AssignBrandCustomer", {
            controller: "AssignBrandCustomerController",
            templateUrl: "/views/Customer/AssignBrandCustomer.html"
        })
        .when("/PointInOut", {
            controller: "PointInOutController",
            templateUrl: "views/Delivery/PointInOut.html"
        })
        .when("/WarehouseExpensePoint", {
            controller: "WarehouseExpensePointController",
            templateUrl: "views/Wallet/WarehouseExpensePoint.html"
        })
        .when("/AssignBulkcustomers", {
            controller: "AssignBulkcustomersController",
            templateUrl: "/views/Customer/AssignBulkcustomers.html"
        })
        .when("/InvoiceBill", {
            controller: "InvoiceBillController",
            templateUrl: "/views/Customer/InvoiceBill.html"
        })
        .when("/CreateCompany", {
            controller: "CreateCompanyController",
            templateUrl: "/views/admin/CreateCompany.html"
        })
        .when("/PendingApproval", {
            controller: "PendingApprovalController",
            templateUrl: "/views/admin/PendingApproval.html"
        })
        .when("/CurrencySettle", {
            controller: "CurrencySettleController",
            templateUrl: "/views/CurrencyModule/Currency.html"
        })
        .when("/BankSettle", {
            controller: "BankSettleController",
            templateUrl: "/views/CurrencyModule/BankSettle.html"
        })
        .when("/CurrencyStock", {
            controller: "CurrencyStockController",
            templateUrl: "views/CurrencyStock/CurrencyStock.html"
        })
        .when("/excelorder", {
            controller: "ExcelOrderCtrl",
            templateUrl: "/views/Order/Excelorder.html"
        })
		.when("/OrederProcessReport", {
		    controller: "OrederProcessReportController",
		    templateUrl: "views/Reports/OrederProcessReport.html"
		})
	    .when("/AssignCustomers", {
	        controller: "AssignCustomersCtrl",
	        templateUrl: "views/suppliers/AssignCustomers.html"
	    })
        .when("/MywarehouseCustomers", {
            controller: "AssignCustomersCtrl",
            templateUrl: "views/suppliers/MyWarehouseCustomer.html"
        })
        .when("/DamageStock", {
            controller: "DamageStockController",
            templateUrl: "views/DamageStock/DamageStock.html"
        })
        .when("/CreateDamageOrder", {
            controller: "CreateDamageOrderController",
            templateUrl: "views/DamageStock/CreateDamageOrder.html"
        })
        .when("/DamageOrder", {
            controller: "DamageorderMasterController",
            templateUrl: "views/DamageStock/DamageorderMaster.html"
        })
        .when("/ProductReport", {
            controller: "ProductReportController",
            templateUrl: "/views/Reports/ProductReport.html"
        })
        .when("/notOrdered", {
            controller: "NotOrderedController",
            templateUrl: "views/Order/Notordered.html"
        })
        .when("/reqService", {
            controller: "ReqServiceController",
            templateUrl: "views/Feed_Report/ReqService.html"
        })
        .when("/CRM", {
            controller: "CRMCtrl",
            templateUrl: "/CRM/CRM.html"
        })
        .when("/    ", {
            controller: "NetprofitController",
            templateUrl: "/CRM/NetProfit.html"
        })
        .when("/filtCustomer", {
            controller: "CRMcust4ActionCtrl",
            templateUrl: "/CRM/CRMcust4Action.html"
        })
        .when("/CustomerService", {
            controller: "customerservices",
            templateUrl: "/views/Customer/CustomerServices.html"
        })
         .when("/customerAdditionalservice", {
             controller: "CustomerAdditionalCtrl",
             templateUrl: "/views/Customer/CustomerAdditionalService.html"
         })
        .when("/MonthlyBill", {
            controller: "MonthlyBillController",
            templateUrl: "/views/Customer/MonthlyBill.html"
        })
         .when("/MonthLeave", {
             controller: "MonthLeaveController",
             templateUrl: "/views/Customer/MonthLeave.html"
         })
        .when("/CustomerStatement", {
            controller: "MonthlyStatementController",
            templateUrl: "/views/Customer/MonthlyStatement.html"
        })
        .when("/CustomerInvoice", {
            controller: "CustomerInvoiceController",
            templateUrl: "/views/Customer/CustomerInvoice.html"
        })
        .when("/CustomerIssue", {
            controller: "CustomerIssueController",
            templateUrl: "/views/Customer/CustomerIssue.html"
        })
        .when("/actionTask", {
            controller: "ActionCtrl",
            templateUrl: "/CRM/Action.html"
        })
        .when("/SupplierIR", {
            controller: "IRSupplierCtrl",
            templateUrl: "/views/IR/IRSupplier.html"
        })
        .when("/SupplierPromo", {
            controller: "SupplierPromoController",
            templateUrl: "/views/ItemCategory/supplierPromo.html"
        })
        .when("/NppHistoryPrice", {
            controller: "NppHistoryPriceController",
            templateUrl: "/views/ItemCategory/NppHistoryPrice.html"
        })
        .when("/redeemMaster", {
            controller: "RedeemOrderCtrl",
            templateUrl: "/views/Order/reedeemOrder.html"
        })
        .when("/Area", {
            controller: "AreaMasterController",
            templateUrl: "/views/WareHouse/Area.html"
        })
        .when("/unitEcoReport", {
            controller: "unitEcoReportCtlr",
            templateUrl: "views/UnitEconomic/ueReport.html"
        })
        .when("/RedeemItem", {
            controller: "RewardItemCtrl",
            templateUrl: "views/Wallet/rewardItem.html"
        })
        .when("/unitEconomic", {
            controller: "unitEcoController",
            templateUrl: "views/UnitEconomic/unitEconomics.html"
        })
        .when("/Promo", {
            controller: "promItemController",
            templateUrl: "views/ItemCategory/promoItems.html"
        })
        .when("/Offer", {
            controller: "OfferController",
            templateUrl: "views/Offer/Offer.html"
        })
        .when("/Reward", {
            controller: "RewardPointController",
            templateUrl: "views/Wallet/Reward.html"
        })
        .when("/Wallet", {
            controller: "WalletController",
            templateUrl: "views/Wallet/Wallet.html"
        })
        .when("/MileStone", {
            controller: "MilestonePointController",
            templateUrl: "views/Wallet/Milestone.html"
        })
        .when("/pages/LogOut", {
            controller: "logoutController",
            templateUrl: "views/pages/logout.html"
        })
        .when("/ShortageSettle", {
            controller: "ShortageSettleController",
            templateUrl: "/views/Order/ShortageSettle.html"
        })
        .when("/returnitem", {
            controller: "ReturnItemCtrl",
            templateUrl: "/views/Reports/ReturnItem.html"
        })
    	.when("/requestbrand", {
    	    controller: "SuggetionContoller",
    	    templateUrl: "views/Feed_Report/suggetion.html"
    	})
        .when("/salestarget", {
            controller: "targetController",
            templateUrl: "views/Feed_Report/SalesTarget.html"
        })
        .when("/BounceCheq", {
            controller: "BounceCheqController",
            templateUrl: "views/SalesSettlement/BounceCheq.html"
        })
        .when("/VehicleAssissment", {
            controller: "VehicleAssissmentController",
            templateUrl: "views/Delivery/VehicleAssissment.html"
        })
        .when("/PendingOrder", {
            controller: "orderPendingController",
            templateUrl: "views/Order/orderPending.html"
        })
        .when("/SalesAsignDay", {
            controller: "AsignDayController",
            templateUrl: "/views/Customer/AsignDays.html"
        })
        .when("/SalesSettlementHistory", {
            controller: "SalesSettlementHistoryController",
            templateUrl: "views/SalesSettlement/SalesSettlementHistory.html"
        })
        .when("/SalesSettlement", {
            controller: "SalesSettlementController",
            templateUrl: "views/SalesSettlement/SalesSettlement.html"
        })
        .when("/SaleCheqBounce", {
            controller: "SalesBounceController",
            templateUrl: "views/SalesSettlement/SaleCheqBounce.html"
        })
        //report
        .when("/report/time", {
            controller: "reportsController",
            templateUrl: "/views/report/time.html"
        })
        .when("/LiveHubKpi", {
            controller: "ReportController",
            templateUrl: "/views/Reports/Report.html"
        })
        .when("/Reports", {
            controller: "Report3Controller",
            templateUrl: "/views/Reports/Report3.html"
        })
        .when("/RetailersReport", {
            controller: "RetailersReportCtrl",
            templateUrl: "/views/Reports/RetailersReport.html"
        })
        .when("/DeliveryBoyReport", {
            controller: "DeliveryBoyReportCtrl",
            templateUrl: "/views/Reports/DeliveryBoyReport.html"
        })
        .when("/Comparison", {
            controller: "ComparisonCtrl",
            templateUrl: "/views/Reports/Comparison.html"
        })
        .when("/HubCity", {
            controller: "Comparison1Ctrl",
            templateUrl: "/views/Reports/Comparison1.html"
        })
        .when("/DashboardReport", {
            controller: "DashboardReportController",
            templateUrl: "/views/Reports/DashboardReport.html"
        })
        .when("/DeliveryBoyHistory", {
            controller: "DeliveryBoyHistoryController",
            templateUrl: "views/Delivery/DeliveryBoyHistory.html"
        })
        .when("/Redispatch", {
            controller: "RedispatchCtrl",
            templateUrl: "views/Redispatch/RedispatchOrders.html"
        })
        .when("/RedispatchAutoCanceled", {
            controller: "RedispatchCtrlAutoCanceled",
            templateUrl: "views/Redispatch/RedispatchOrdersAutoCanceled.html"
        })
        .when("/deliveryCharge", {
            controller: "deliveryChargeController",
            templateUrl: "views/admin/DeliveryCharges.html"
        })
        .when("/ChangeDBoy", {
            controller: "ChangeDBoyCtrl",
            templateUrl: "views/Delivery/ChangeDBoy.html"
        })
        .when("/News", {
            controller: "NewsController",
            templateUrl: "views/admin/News.html"
        })
        .when("/MyOffer", {
            controller: "CouponController",
            templateUrl: "views/admin/Coupon.html"
        })
        .when("/Vehicles", {
            controller: "VehicleController",
            templateUrl: "views/Delivery/Vehicle.html"
        })
        .when("/DeliveryBoy", {
            controller: "DeliveryBoyController",
            templateUrl: "views/Delivery/DeliveryBoy.html"
        })
        .when("/Delivery", {
            controller: "DeliveryController",
            templateUrl: "views/Delivery/Delivery.html"
        })
        .when("/pages/confirmEmail", {
            controller: "confirmEmailController",
            templateUrl: "views/pages/ConfirmEmail.html"
        })
        .when("/map", {
            controller: "mappController",
            templateUrl: "/views/new/mapp.html"
        })
        .when("/PurchaseOrderdetails", {
            controller: "SearchPODetailsController",
            templateUrl: "/views/Reports/PurchaseOrderdetails.html"
        })
        .when("/PurchaseInvoice", {
            controller: "SearchPODetailsController",
            templateUrl: "/views/invoice/PurchaseInvoice.html"
        })
        .when("/goodsrecived", {
            controller: "GoodsRecivedController",
            templateUrl: "/views/reports/GoodsRecived.html"
        })
        .when("/IR", {
            controller: "IRController",
            templateUrl: "/views/IR/IR.html"
        })
        .when("/CurrentStock", {
            controller: "CurrentStockController",
            templateUrl: "/views/Stock/CurrentStock.html"
        })
        //By Sachin In CurrentStockController
        .when("/EmptyStockItem", {
            controller: "EmptyCurrentController",
            templateUrl: "/views/Stock/EmptyStockItem.html"
        })
        .when("/SearchPurchaseOrder", {
            controller: "searchPOController",
            templateUrl: "/views/Reports/SearchPurchaseOrder.html"
        })
        .when("/WarehouseSupplier", {
            controller: "WarehouseSupplierController",
            templateUrl: "/views/WareHouse/WarehouseSupplier.html"
        })
        .when("/Orderdetails", {
            controller: "orderdetailsController",
            templateUrl: "/views/Order/Orderdetails.html"
        })
        .when("/ReturnOrderdetails", {
            controller: "ReturnOrderdetailsController",
            templateUrl: "/views/Order/ReturnOrderdetails.html"
        })
        .when("/viewpurchase", {
            controller: "purchaseorderController",
            templateUrl: "/views/Reports/PurchaseOrder.html"
        })
        .when("/PurchaseOrderList", {
            controller: "PurchaseOrderListController",
            templateUrl: "/views/Reports/PurchaseOrderList.html"
        })
        .when("/invoice", {
            controller: "orderdetailsController",
            templateUrl: "/views/invoice/invoice.html"
        })
        .when("/demand", {
            controller: "demandController",
            templateUrl: "/views/WareHouse/AddDemand.html"
        })
        .when("/demandreport", {
            controller: "demandController",
            templateUrl: "/views/Reports/DemandReport.html"
        })
        .when("/pramotion", {
            controller: "PramotionController",
            templateUrl: "/views/Pramotion/ItemPramotion.html"
        })
        .when("/billpramotion", {
            controller: "BillPramotionController",
            templateUrl: "/views/Pramotion/BillPramotion.html"
        })
        .when("/brandpramotion", {
            controller: "PramotionController",
            templateUrl: "/views/Pramotion/BrandPramotion.html"
        })
        .when("/unitMaster", {
            controller: "unitMasterController",
            templateUrl: "/views/new/UnitMasterCategory.html"
        })
        //.when("/orderMaster", {
        //    controller: "orderMasterController",
        //    templateUrl: "/views/Order/orderMaster.html"
        //})
        .when("/orderSettle", {
            controller: "OrderSettleController",
            templateUrl: "/views/Order/OrderSettle.html"
        })
        .when("/ItemMaster", {
            controller: "ItemMasterNewController",
            templateUrl: "/views/ItemMaster/itemMaster.html"
        })
        //.when("/itemMaster", {
        //    controller: "itemMasterController",
        //    templateUrl: "/views/ItemCategory/itemMaster.html"
        //})
        .when("/MOQItemMaster", {
            controller: "MOQItemMasterController",
            templateUrl: "/views/ItemCategory/MOQItemMaster.html"
        })
        .when("/ItemsSupplierMapping", {
            controller: "ItemSupplierMappingController",
            templateUrl: "/views/ItemCategory/ItemSupplierMapping.html"
        })
        .when("/HighestDreamPoint", {
            controller: "HighestDreamPointItemController",
            templateUrl: "/views/ItemCategory/HighestDreamPointItem.html"
        })
        .when("/editPrice", {
            controller: "editPriceController",
            templateUrl: "/views/ItemCategory/editPrice.html"
        })
        .when("/MoveItems", {
            controller: "CopyItemController",
            templateUrl: "/views/MoveItems/MoveItems.html"
        })
        .when("/MoveSubCategory", {
            controller: "CopyItemController",
            templateUrl: "/views/MoveItems/MoveSubCategory.html"
        })
        .when("/MoveCategory", {
            controller: "CopyItemController",
            templateUrl: "/views/MoveItems/MoveCategory.html"
        })
        .when("/MoveSub2Category", {
            controller: "CopyItemController",
            templateUrl: "/views/MoveItems/MoveSub2Category.html"
        })
        .when("/FilterItem", {
            controller: "FilterItemsController",
            templateUrl: "/views/ItemCategory/FilterItem.html"
        })
        .when("/TaxGroup", {
            controller: "TaxGroupController",
            templateUrl: "/views/TaxGroup/TaxGroup.html"
        })
        .when("/TaxMaster", {
            controller: "TaxmasterController",
            templateUrl: "/views/TaxGroup/TaxMaster.html"
        })
        .when("/Itembrandname", {
            controller: "itembrandController",
            templateUrl: "/views/new/Itembrandname.html"
        })
        .when("/Slider", {
            controller: "SliderCtrl",
            templateUrl: "/views/Slider/Slider.html"
        })
        .when("/userdashboard", {
            controller: "userdashboardController",
            templateUrl: "views/report/userdashboard.html"
        })
        .when("/clientdashboard/:id", {
            controller: "clientdashboardController",
            templateUrl: "views/report/clientdashboard.html"
        })
        .when("/projectdashboard/:id", {
            controller: "projectdashboardController",
            templateUrl: "views/report/projectdashboard.html"
        })
        .when("/taskdashboard/:id", {
            controller: "taskdashboardController",
            templateUrl: "views/report/taskdashboard.html"
        })
        .when("/settings", {
            controller: "settingsController",
            templateUrl: "/views/admin/setting.html"
        })
        .when("/settings/settingEdit", {
            controller: "settingsController",
            templateUrl: "/views/admin/settingEdit.html"
        })
        .when("/customers", {
            controller: "customerController",
            templateUrl: "/views/Customer/customers.html"
        })
          .when("/DeliveryChallan", {
              controller: "DeliveryChallanController",
              templateUrl: "/views/admin/DeliveryChallan.html"
          })
        .when("/Authcustomers", {
            controller: "AuthcustomerController",
            templateUrl: "/views/Customer/Authcustomers.html"
        })
        //.when("/Message", {
        //    controller: "MessageController",
        //    templateUrl: "/views/admin/Message.html"
        //})
        .when("/projects", {
            controller: "projectsController",
            templateUrl: "/views/admin/projects.html"
        })
        .when("/projecttasks", {
            controller: "tasksController",
            templateUrl: "/views/admin/projectsTasks.html"
        })
        .when("/tasktypes", {
            controller: "tasktypesController",
            templateUrl: "/views/admin/tasktypes.html"
        })
        .when("/assetsCategory", {
            controller: "assetsCategoryController",
            templateUrl: "/views/admin/assetsCategory.html"
        })
        .when("/assets", {
            controller: "assetsController",
            templateUrl: "/views/admin/assets.html"
        })
        //.when("/role", {
        //     controller: "roleController",
        //     templateUrl: "/views/admin/Roles.html "
        // })
        .when("/people", {
            controller: "peoplesController",
            templateUrl: "/views/admin/people.html"
        })
        .when("/NotificationByDeviceId", {
            controller: "NotificationByDeviceIdController",
            templateUrl: "/views/notification/NotificationByDeviceId.html"
        })
        .when("/Notification", {
            controller: "NotificationController",
            templateUrl: "/views/notification/Notification.html"
        })
        .when("/expenseReport", {
            controller: "expensesController",
            templateUrl: "/views/admin/expenseReport.html"
        })
        .when("/leaveReport", {
            controller: "leavesController",
            templateUrl: "/views/admin/leaveReport.html"
        })
        .when("/expenses", {
            controller: "expensesController",
            templateUrl: "/views/admin/expenses.html"
        })
        .when("/expensecat", {
            controller: "expensecatController",
            templateUrl: "/views/admin/expensecat.html"
        })
        .when("/tfsweektimesheet", {
            controller: "TFSweektimesheetController",
            templateUrl: "/views/timesheet/tfsweektimesheet.html"
        })
        .when("/weektimesheet", {
            controller: "weektimesheetController",
            templateUrl: "/views/timesheet/weektimesheet.html"
        })
        .when("/weektasktimesheet", {
            controller: "weektasktimesheetController",
            templateUrl: "/views/timesheet/weektasktimesheet.html"
        })
        .when("/daytimesheet", {
            controller: "daytimesheetController",
            templateUrl: "/views/timesheet/daytimesheet.html"
        })
        .when("/daytasktimesheet", {
            controller: "daytasktimesheetController",
            templateUrl: "/views/timesheet/daytasktimesheet.html"
        })
        .when("/monthtimesheet", {
            controller: "monthtimesheetController",
            templateUrl: "/views/timesheet/monthtimesheet.html"
        })
        .when("/leave", {
            controller: "leavesController",
            templateUrl: "/views/timesheet/leavepage.html"
        })
        .when("/leaveApprove", {
            controller: "leavesController",
            templateUrl: "/views/admin/leaveApprove.html"
        })
        .when("/expenseApprove", {
            controller: "expensesController",
            templateUrl: "/views/admin/expenseApprove.html"
        })
        .when("/travelRequestApprove", {
            controller: "travelRequestController",
            templateUrl: "/views/admin/travelRequestApprove.html"
        })
        .when("/travelRequestPage", {
            controller: "travelRequestController",
            templateUrl: "/views/timesheet/travelRequestPage.html"
        })
        //.when("/supplierCategory", {
        //    controller: "supplierCategoryController",
        //    templateUrl: "/views/suppliers/supplierCategory.html"
        //})
        //.when("/supplier", {
        //    controller: "supplierController",
        //    templateUrl: "/views/suppliers/supplier.html"
        //})
        //.when("/CustomerCategory",
        // {
        //     controller: "customerCategoryController",
        //     templateUrl: "/views/Customer/CustomerCategory.html"
        // })
        .when("/state", {
            controller: "stateController",
            templateUrl: "/views/new/State.html"
        })
        .when("/city", {
            controller: "cityController",
            templateUrl: "/views/new/City.html"
        })
        //.when("/Multipleimage", {
        //     controller: "multipleimageController",
        //     templateUrl: "/views/NewFolder1/multipleimages.html"
        // })
        .when("/warehouse", {
            controller: "warehouseController",
            templateUrl: "/views/WareHouse/Warehouse.html"
        })
        .when("/cluster", {
            controller: "clusterController",
            templateUrl: "/views/WareHouse/Cluster.html"
        })
        .when("/WarehouseCategory", {
            controller: "WarehousecategoryController",
            templateUrl: "/views/WareHouse/WarehouseCategory.html"
        })
        .when("/WarehouseSubCategory", {
            controller: "WarehousesubCategoryController",
            templateUrl: "/views/WareHouse/WarehousesubCategory.html"
        })
        .when("/WarehouseSubsubCategory", {
            controller: "WarehousesubsubCategoryController",
            templateUrl: "/views/WareHouse/WarehouseSubsubCategory.html"
        })
         .when("/Depot", {
             controller: "DepotController",
             templateUrl: "/views/WareHouse/Depot.html"
         })
        .when("/Zone", {
            controller: "ZoneController",
            templateUrl: "/views/WareHouse/Zone.html"
        })
        .when("/basecategory", {
            controller: "basecategoryController",
            templateUrl: "/views/Category/BaseCategory.html"
        })
        .when("/category", {
            controller: "categoryController",
            templateUrl: "/views/Category/Category.html"
        })
        .when("/subCategory", {
            controller: "subcategoryController",
            templateUrl: "/views/Category/subCategory.html"
        })
        .when("/subsubcategory", {
            controller: "subsubCategoryController",
            templateUrl: "/views/Category/SubsubCategory.html"
        })
        //By Sachin For Intigrating Tally
        .when("/Tally", {
            controller: "TallyController",
            templateUrl: "/views/admin/Tally.html"
        })
        .when("/PrestloanCustomer", {
            controller: "PrestloanCustomerController",
            templateUrl: "/views/Customer/PrestloanCustomer.html"
        })
        .when("/warehousesubsubcategory", {
            controller: "WarehousesubsubCategoryController",
            templateUrl: "/views/WareHouse/WarehouseSubsubCategory.html"
        })
        .when("/OrderPaymentReport", {
            controller: "OrderPaymentReportController",
            templateUrl: "/views/Reports/OrderPaymentReport.html"
        })
        .when("/invoice/creatingRecurringinvoice", {

            templateUrl: "/views/invoice/creatingRecurringinvoice.html"
        })
        .when("/invoice/3", {

            templateUrl: "/views/invoice/recurring.html"
        })
        .when("/ui/typography", {
            templateUrl: "views/ui/typography.html"
        }).when("/ui/buttons", {
            templateUrl: "views/ui/buttons.html"
        }).when("/ui/icons", {
            templateUrl: "views/ui/icons.html"
        }).when("/ui/grids", {
            templateUrl: "views/ui/grids.html"
        }).when("/ui/widgets", {
            templateUrl: "views/ui/widgets.html"
        }).when("/ui/components", {
            templateUrl: "views/ui/components.html"
        }).when("/ui/timeline", {
            templateUrl: "views/ui/timeline.html"
        }).when("/ui/pricing-tables", {
            templateUrl: "views/ui/pricing-tables.html"
        }).when("/forms/elements", {
            templateUrl: "views/forms/elements.html"
        }).when("/forms/layouts", {
            templateUrl: "views/forms/layouts.html"
        }).when("/forms/validation", {
            templateUrl: "views/forms/validation.html"
        }).when("/forms/wizard", {
            templateUrl: "views/forms/wizard.html"
        }).when("/tables/static", {
            templateUrl: "views/tables/static.html"
        }).when("/tables/responsive", {
            templateUrl: "views/tables/responsive.html"
        }).when("/tables/dynamic", {
            templateUrl: "views/tables/dynamic.html"
        }).when("/charts/others", {
            templateUrl: "views/charts/charts.html"
        }).when("/charts/morris", {
            templateUrl: "views/charts/morris.html"
        }).when("/charts/flot", {
            templateUrl: "views/charts/flot.html"
        }).when("/mail/inbox", {
            templateUrl: "views/mail/inbox.html"
        }).when("/mail/compose", {
            templateUrl: "views/mail/compose.html"
        }).when("/mail/single", {
            templateUrl: "views/mail/single.html"
        }).when("/pages/features", {
            templateUrl: "views/pages/features.html"
        })
        .when("/pages/signin", {
            controller: "loginController",
            templateUrl: "views/pages/signin.html"

        }).when("/pages/signup", {
            controller: "signupController",
            templateUrl: "views/pages/signup.html"

        }).when("/pages/OTPSignUp", {
            controller: "OTPsignupController",
            templateUrl: "views/pages/otp.html"

        }).when("/pages/OTPVerify", {
            controller: "OTPsignupController",
            templateUrl: "views/pages/otpVerify.html"

        }).when("/pages/lock-screen", {
            templateUrl: "views/pages/lock-screen.html"
        }).when("/pages/profile", {
            controller: "profilesController",
            templateUrl: "views/pages/profile.html"
        })
        .when("/pages/profile/profileEdit", {
            controller: "profilesController",
            templateUrl: "views/pages/profileEdit.html"
        })
        .when("/AppPromotion", {
            controller: "MarginImagePromotionController",
            templateUrl: "/views/AppPromotion/MarginImagePromotion.html"
        })
        //By Sachin This Controller is in MarginImagePromotionController
        .when("/TopAddedItem", {
            controller: "MarginImagePromotionController",
            templateUrl: "/views/AppPromotion/TopAddedItem.html"
        })
         //By Sachin This Controller is in MarginImagePromotionController
        .when("/TotalDhamaka", {
            controller: "TotalDhamakaController",
            templateUrl: "/views/AppPromotion/TodayDhamaka.html"
        })
        //By Sachin This Controller is in MarginImagePromotionController
        .when("/BulkDeal", {
            controller: "BulkDealController",
            templateUrl: "/views/AppPromotion/BulkDeal.html"
        })
        .when("/MostSelledItem", {
            controller: "MostSelledItemController",
            templateUrl: "/views/AppPromotion/MostSelledItem.html"
        })
        .when("/MostSelledBrand", {
            controller: "MostSelledBrandController",
            templateUrl: "/views/AppPromotion/MostSelledBrand.html"
        })
        .when("/categoryImage", {
            controller: "CategoryImageController",
            templateUrl: "/views/Category/CategoryImage.html"
        })
        .when("/AgentDashboard", {
            controller: "AgentsDashboardController",
            templateUrl: "/views/admin/AgentDashboard.html"
        })
        .when("/AgentExcel", {
            controller: "AgentExcelController",
            templateUrl: "/views/admin/AgentExcel.html"
        })
        .when("/TransferOrderSend", {
            controller: "TransferOrderController",
            templateUrl: "/views/TransferOrder/TransferOrderSend.html"
        })
        .when("/TransferOrderRequest", {
            controller: "TransferOrderRequestController",
            templateUrl: "/views/TransferOrder/TransferOrderRequest.html"
        })
        .when("/UserAccessPermission", {
            controller: "UserAccessPermissionController",
            templateUrl: "/views/admin/UserAccessPermission.html"
        })
         .when("/Murli", {
             controller: "CustomerVoiceController",
             templateUrl: "/views/admin/Murli.html"
         })
        .when("/Case", {
            controller: "casecontroller",
            templateUrl: "/views/admin/Case.html"
        })
        .when("/pages/blank", {
            templateUrl: "views/pages/blank.html"
        })
        .when("/pages/invoice", {
            templateUrl: "views/pages/invoice.html"
        })
        .when("/trackuser", {
            controller: "trackuser",
            templateUrl: "/views/admin/trackuser.html"
        })
        .when("/tasks", {
            templateUrl: "views/tasks/tasks.html"
        })
        .when("/404", {
            templateUrl: "views/pages/404.html"
        })
        .when("/pages/500", {
            templateUrl: "views/pages/500.html"
        })
        .otherwise({
            redirectTo: "/404"
        })
}]);
var serviceBase = 'http://localhost:26265/';  //Local    
//var serviceBase = 'https://obhaiya.azurewebsites.net/';   //Testing  
//var serviceBase = 'http://137.59.52.130/';    //Live
//var serviceBase = 'http://103.73.190.66:8787/';  //Live on IIS
//var serviceBase = 'http://103.73.190.66:808/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});
app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});
app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);
app.directive("uiSpinner", [function () {
    return {
        restrict: "A",
        compile: function (ele) {
            return ele.addClass("ui-spinner"), {
                post: function () {
                    return ele.spinner()
                }
            }
        }
    }

    $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));
}]);