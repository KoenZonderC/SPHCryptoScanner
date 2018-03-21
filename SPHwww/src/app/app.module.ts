import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialDesignModule } from './materialdesign.module';
import { AppComponent } from './app.component';

import { TradingViewComponent } from './components/tradingview.component';

@NgModule({
  declarations: [

    // app
    AppComponent,

    // components
    TradingViewComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MaterialDesignModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
