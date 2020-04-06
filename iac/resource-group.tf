resource "azurerm_resource_group" "rg" {
  name     = join("", [var.projectname, "-rg"])
  location = var.location
}