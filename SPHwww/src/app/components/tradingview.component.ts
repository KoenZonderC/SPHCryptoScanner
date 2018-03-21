import { ElementRef, Input, Component, OnInit } from '@angular/core';
import * as $ from 'jquery';
declare const TradingView: any;

@Component({
  selector: 'tradingview',
  templateUrl: './tradingview.component.html',
})
export class TradingViewComponent implements OnInit {
  @Input("symbol") set SetSymbol(symbol: string) {
    this.symbol = symbol;
    if (this.widget != null)
    {
      console.log("change symbol" + this.widget);
      var chart = this.widget.activeChart();
      console.log("chart" + chart);
      this.widget.activeChart().setSymbol(this.symbol, "60", () => {
        console.log("switched symbol");
      });
    }
  }

  private symbol: string = "BTCUSD";
  private widget: any = null;

  constructor(private elementRef: ElementRef) {
    console.log("ctor");

  }

  ngOnInit() {
    console.log("Create widget");
    var widget=new TradingView.widget(
      {
        "width": 980,
        "height": 610,
        "symbol": this.symbol,
        "interval": "60",
        "timezone": "Etc/UTC",
        "theme": "Light",
        "style": "1",
        "locale": "en",
        "toolbar_bg": "#f1f3f6",
        "enable_publishing": false,
        "allow_symbol_change": true,
        "hideideas": true,
        "container_id": "tradingview_f16be"
      }
    );
    widget.onChartReady(function () {
      console.log("--on chart ready--");
    });
  }
}
