terraform {
  required_version = ">=0.12"
}

provider "azurerm" {
  version = ">=2.0.0"
  features {}
}

variable "resource_group_name" {
  type = string
  default = "terraform-backend-rg"
}

variable "location" {
  type        = string
  default = "uksouth"
}

resource "azurerm_resource_group" "resource-group" {
  name     = var.resource_group_name
  location = var.location
}

resource "random_string" "unique" {
  length  = 4
  number  = false
  upper   = false
  special = false
}

resource "azurerm_storage_account" "storage" {
  name                     = join("", ["terraformbackend", random_string.unique.result])
  resource_group_name      = azurerm_resource_group.resource-group.name
  location                 = azurerm_resource_group.resource-group.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  account_kind             = "StorageV2"
  access_tier              = "Cool"
}

resource "azurerm_storage_container" "container" {
  name                  = "megastore"
  storage_account_name  = azurerm_storage_account.storage.name
  container_access_type = "private"
}

output "backend_storage_account_name" {
  value = azurerm_storage_account.storage.name
}

output "backend_storage_access_key" {
  value = azurerm_storage_account.storage.primary_access_key
}

output "backend_storage_container_name" {
  value = azurerm_storage_container.container.name
}
