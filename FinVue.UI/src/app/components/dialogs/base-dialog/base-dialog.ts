import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, ComponentRef, Inject, OnChanges, OnDestroy, signal, SimpleChanges, ViewChild, ViewContainerRef } from '@angular/core';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatIcon } from '@angular/material/icon';
import { catchError, Observable, Subscription } from 'rxjs';
import { LoadingSpinner } from "../../loading-spinner/loading-spinner";

export interface IReturnData {
    getReturnData() : any;
}
export interface ISaveData {
    onSave() : Observable<any>;
}

function isIReturnData(arg: any): arg is IReturnData {
    return arg.getReturnData !== undefined;
}
function isISaveData(arg : any): arg is ISaveData {
    return arg.onSave !== undefined;
}

@Component({
  selector: 'app-base-dialog',
  imports: [MatIcon, MatIconButton, MatButton, CommonModule, LoadingSpinner],
  templateUrl: './base-dialog.html',
  styleUrl: './base-dialog.scss'
})
export class BaseDialog implements AfterViewInit, OnDestroy {
    @ViewChild('contentContainer', { read: ViewContainerRef }) container!: ViewContainerRef;

    private componentRef?: ComponentRef<any>;
    private saveSubscription?: Subscription;

    public static ACTION_CANCEL = 'cancel';
    public static ACTION_CLOSE = 'close';
    public static ACTION_SAVE = 'save';

    public dialogTitle? : string;
    public loading = signal(false);

    constructor(
        public dialogRef: MatDialogRef<BaseDialog>,
        @Inject(MAT_DIALOG_DATA) public data: { component: any, componentTitle?: string, componentData?: any }
    ) {
        this.dialogTitle = data.componentTitle;
    }
  
    ngAfterViewInit() {
        if(!this.data?.component) {
            this.dialogRef.close();
        }
        
        this.componentRef = this.container.createComponent(this.data.component);
        if (this.data.componentData) {
            Object.assign(this.componentRef.instance, this.data.componentData);
        }
    }

    ngOnDestroy(): void {
        this.saveSubscription?.unsubscribe();
    }
  
    onCancel(): void {
        this.dialogRef.close({ action: BaseDialog.ACTION_CANCEL });
    }
  
    onSave(): void {
        const data : any= {
            returnData: this.getReturnData()
        }
        if(isISaveData(this.componentRef?.instance)) {
            console.log("starting save");
            this.loading.set(true);
            this.saveSubscription = this.componentRef?.instance.onSave()
                .subscribe({
                    next: e => {
                        console.log("saved, closing...")
                        data.saveData = e;
                        this.dialogRef.close({ action: BaseDialog.ACTION_SAVE, data: data });
                    },
                    error: error => {
                        console.log(error)
                        this.loading.set(false);
                        return;
                    }
                })

            return;
        }

        
        this.dialogRef.close({ action: BaseDialog.ACTION_SAVE, data: data });
    }

    getReturnData() : any|undefined {
        let returnData;
        if(isIReturnData(this.componentRef?.instance)) {
            returnData = this.componentRef.instance.getReturnData();
        }
        return returnData;
    }
  
    onClose(): void {
        this.dialogRef.close({ action: BaseDialog.ACTION_CLOSE });
    }
}
