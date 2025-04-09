terraform {
  required_providers {
    digitalocean = {
      source  = "digitalocean/digitalocean"
      version = "~> 2.0"
    }
  }
}

provider "digitalocean" {
  token = var.do_token
}
resource "digitalocean_droplet" "vps" {
  name     = "terraform-test-v1"
  region   = "fra1"
  size     = "s-1vcpu-1gb"
  image    = "ubuntu-22-04-x64"
  ssh_keys = [var.ssh_fingerprint]
}

output "vps_ip" {
  value = digitalocean_droplet.vps.ipv4_address
}