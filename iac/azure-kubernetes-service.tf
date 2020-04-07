resource "azurerm_kubernetes_cluster" "aks" {
  name                = join("", [var.project_name, "-aks"])
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  dns_prefix          = join("", [var.project_name, "-aks"])

  default_node_pool {
    name       = "agentpool"
    node_count = 1
    vm_size    = "Standard_B2s"
  }

  service_principal {
    client_id     = var.aks_client_id
    client_secret = var.aks_client_secret
  }

  addon_profile {
    kube_dashboard {
      enabled = true
    }
  }
}