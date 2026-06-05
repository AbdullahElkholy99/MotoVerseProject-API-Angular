import { Component } from '@angular/core';
import { RouterOutlet } from "@angular/router";
import { Navbar } from "../../Bases/navbar/navbar";
import { Footer } from "../../Bases/footer/footer";

@Component({
  selector: 'app-landing-page',
  imports: [RouterOutlet, Navbar, Footer],
  templateUrl: './landing-page.html',
  styleUrls: ['./landing-page.css', `../../styleBoke.css`],
})
export class LandingPage {}
