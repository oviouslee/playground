import {
  Injectable,
  Optional
} from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { SnackerService } from '..';
import { ServerConfig } from '../../config';
import { Diff } from '../../models';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DiffService {
  private diff = new BehaviorSubject<Diff>(null);
  diff$ = this.diff.asObservable();

  constructor(
    private http: HttpClient,
    private snacker: SnackerService,
    @Optional() private config: ServerConfig
  ) { }

  getDiff = (id: number): Promise<Diff> => new Promise((resolve) => {
    const observer = {
      next: data => {
        resolve(data);
        this.diff.next(data);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(null);
      }
    };

    this.http.get<Diff>(`${this.config.api}diff/getDiff/${id}`)
      .subscribe(observer)
  })

  removeParagraph = (d: Diff): Promise<boolean> => new Promise((resolve) => {
    const observer = {
      next: () => {
        this.snacker.sendSuccessMessage(`Diff:, ${d.type} is permanently removed`);
        resolve(true);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(false);
      }
    };

    this.http.post(`${this.config.api}diff/removeDiff`, d)
      .subscribe(observer)
  })

  addDiff = (d: Diff): Promise<boolean> => new Promise((resolve) => {
    const observer = {
      next: data => {
        this.snacker.sendSuccessMessage(`Diff: ${d.type} is successfully created`);
        resolve(data);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(false);
      }
    };

    this.http.post(`${this.config.api}diff/addDiff`, d)
      .subscribe(observer)
  })

  updateDiff = (d: Diff): Promise<boolean> => new Promise((resolve) => {
    const observer = {
      next: data => {
        this.snacker.sendSuccessMessage(`Diff: ${d.type} has successfully updated`);
        resolve(data);
      },
      error: err => {
        this.snacker.sendErrorMessage(err.error);
        resolve(false);
      }
    };

    this.http.post(`${this.config.api}diff/updateDiff`, d)
      .subscribe(observer)
  })
}
