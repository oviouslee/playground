import { Change } from '..';

export interface Diff {
  id: number,
  changeId: number,
  type: string,
  previous: string,
  proposed: string,

  change: Change
}
