resource "azurerm_application_insights" "aai" {

  for_each            = var.environments_list
  name                = join("", [var.projectname, "-", each.value, "-aai"])
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  application_type    = "web"
}