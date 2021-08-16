import {
  Injectable,
  Optional
} from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { SnackerService } from '..';
import { ServerConfig } from '../../config';
import { Conflict } from '../../models';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConflictService {
  private conflict = new BehaviorSubject<Conflict>(null);
  conflict$ = this.conflict.asObservable();

  constructor(
    private http: HttpClient,
    private snacker: SnackerService,
    @Optional() private config: ServerConfig
  ) { }

  getConflict = (id: number): Promise<Conflict> => new Promise((resolve) => {
    const observer = {
      next: data => {
        resolve(data);
        this.conflict.next(data);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(null);
      }
    };

    this.http.get<Conflict>(`${this.config.api}conflict/getConflict/${id}`)
      .subscribe(observer)
  })

  removeConflict = (c: Conflict): Promise<boolean> => new Promise((resolve) => {
    const observer = {
      next: () => {
        this.snacker.sendSuccessMessage(`Conflict:, ${c.id} is permanently removed`);
        resolve(true);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(false);
      }
    };

    this.http.post(`${this.config.api}conflict/removeConflict`, c)
      .subscribe(observer)
  })

  addConflict = (c: Conflict): Promise<boolean> => new Promise((resolve) => {
    const observer = {
      next: data => {
        this.snacker.sendSuccessMessage(`Conflict: ${c.id} is successfully created`);
        resolve(data);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(false);
      }
    };

    this.http.post(`${this.config.api}conflict/addConflict`, c)
      .subscribe(observer)
  })

  updateConflict = (c: Conflict): Promise<boolean> => new Promise((resolve) => {
    const observer = {
      next: data => {
        this.snacker.sendSuccessMessage(`Conflict: ${c.id} has successfully updated`);
        resolve(data);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(false);
      }
    };

    this.http.post(`${this.config.api}conflict/updateConflict`, c)
      .subscribe(observer)
  })
}
