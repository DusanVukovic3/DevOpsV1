import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTreeFlatDataSource, MatTreeFlattener, MatTreeModule } from '@angular/material/tree';
import { FlatTreeControl } from '@angular/cdk/tree';
import { AuthService } from '../service/auth.service';
import { User } from '../model/user.model';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

interface UserNode {
  name: string;
  movies: UserNode[];
}

interface ExampleFlatNode {
  expandable: boolean;
  name: string;
  level: number;
}

@Component({
  selector: 'app-users-see-collection',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule, MatTreeModule],
  providers: [],
  templateUrl: './users-see-collection.component.html',
  styleUrl: './users-see-collection.component.css'
})
export class UsersSeeCollectionComponent implements OnInit {


  users: User[] = []
  transformedData: UserNode[] = [];
  private _transformer = (node: UserNode, level: number) => {
    return {
      expandable: !!node.movies && node.movies.length > 0,
      name: node.name,
      level: level,
    };
  };

  treeControl = new FlatTreeControl<ExampleFlatNode>(
    node => node.level,
    node => node.expandable,
  );

  treeFlattener = new MatTreeFlattener(
    this._transformer,
    node => node.level,
    node => node.expandable,
    node => node.movies,
  );

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  constructor(private authService: AuthService, public dialogRef: MatDialogRef<UsersSeeCollectionComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.getAllUsers();
  }

  getAllUsers(): void {
    this.authService.getAllExceptLogged(this.data.username).subscribe(res => {
      this.users = res;
      this.transformedData = this.users.map(user => ({
        name: user.username,
        movies: user.movies.map(movie => ({
          name: movie.name,
          movies: []  // No nested movies
        })),
      }));
      this.dataSource.data = this.transformedData;
      console.log(this.transformedData);
    });
  }

  hasChild = (_: number, node: ExampleFlatNode) => node.expandable;

  closeDialog() {
    this.dialogRef.close();
  }

}
