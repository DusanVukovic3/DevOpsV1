# Little devOps playground
For the base of this project, i used my other project "MovieCollectionAPI" which is a basic fullstack project with a database. Then, i tried to automate every part of that web application. Firstly, i used TerraScape to make an initial structure. After that, I hosted my front-end code on netlify.com, which is free. Later, I found a VPS (virtual private server) from digitalOcean, where I put my back-end code and my database (postgreSQL). Lastly, I made a production version of my code, meaning, my back-end can be used as a linux service, or rather, as an API. 

After that, i added Ansible, so i could automate everything. Firstly, netlify.com automatically updates my FE code when i push an update using github (netlify looks at the pull requests and you can configure which branch it should follow for updates). With ansible and github Actions, i automated my back-end code, so every time i make a change on MAIN branch, ansible immediatelly pulls the newest version of my code (using github) and then makes a new production version of my code (more precisely, makes a new updated linux service, which my front-end can then use).



