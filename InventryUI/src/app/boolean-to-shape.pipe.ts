import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'booleanToShape'
})
export class BooleanToShapePipe implements PipeTransform {
  transform(value: boolean, trueShape: string =  '🟢', falseShape: string = '🔴'): string {
    return value ? trueShape : falseShape;
  }
}
