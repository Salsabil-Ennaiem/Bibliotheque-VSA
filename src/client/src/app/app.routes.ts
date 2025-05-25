import { Routes } from '@angular/router';
import { AccueilComponent } from './pages/accueil/accueil.component';
import { CompteComponent } from './pages/compte/compte.component';
import { LoginComponent } from './pages/compte/login/login.component';
import { MdpOublieeComponent } from './pages/compte/mdp-oubliee/mdp-oubliee.component';
import { BibliothecaireComponent } from './pages/bibliothecaire/bibliothecaire.component';
import { TableauxDeBordComponent } from './pages/bibliothecaire/tableaux-de-bord/tableaux-de-bord.component';
import { NouveauteComponent } from './pages/bibliothecaire/nouveaute/nouveaute.component';
import { ListeNouveauteComponent } from './pages/bibliothecaire/nouveaute/liste-nouveaute/liste-nouveaute.component';
import { AjouterNouveauteComponent } from './pages/bibliothecaire/nouveaute/ajouter-nouveaute/ajouter-nouveaute.component';
import { ModifierNouveauteComponent } from './pages/bibliothecaire/nouveaute/modifier-nouveaute/modifier-nouveaute.component';
import { NouveauteDetailsComponent } from './pages/bibliothecaire/nouveaute/nouveaute-details/nouveaute-details.component';
import { EmpruntsComponent } from './pages/bibliothecaire/emprunts/emprunts.component';
import { ListeEmpruntsComponent } from './pages/bibliothecaire/emprunts/liste-emprunts/liste-emprunts.component';
import { AjoutEmpruntsComponent } from './pages/bibliothecaire/emprunts/ajout-emprunts/ajout-emprunts.component';
import { ModifierEmpruntsComponent } from './pages/bibliothecaire/emprunts/modifier-emprunts/modifier-emprunts.component';
import { LivresComponent } from './pages/bibliothecaire/livres/livres.component';
import { ListeLivresComponent } from './pages/bibliothecaire/livres/liste-livres/liste-livres.component';
import { AjoutLivresComponent } from './pages/bibliothecaire/livres/ajout-livres/ajout-livres.component';
import { ModifierLivresComponent } from './pages/bibliothecaire/livres/modifier-livres/modifier-livres.component';
import { ErreurComponent } from './pages/erreur/erreur.component';
import { SanctionsComponent } from './pages/bibliothecaire/sanctions/sanctions.component';
import { AjoutSanctionComponent } from './pages/bibliothecaire/sanctions/ajout-sanction/ajout-sanction.component';
import { ListSanctionsComponent } from './pages/bibliothecaire/sanctions/list-sanctions/list-sanctions.component';


export const routes: Routes = [
    { path: 'accueil', component: AccueilComponent },
    {path:'compte', component:CompteComponent ,
      children:[
        {path:'login',component:LoginComponent } ,
        {path:'motDePasseOublie',component:MdpOublieeComponent} ,
      ]
    },
    {path:'bibliothecaire' , component:BibliothecaireComponent ,
      children:[
        {path:'',component:TableauxDeBordComponent},
        {path:'sanctions' , component:SanctionsComponent ,
          children:[
             {path:'',component:ListSanctionsComponent },
            {path:'ajouter',component:AjoutSanctionComponent }
          ]
        } ,
        {path:'nouveaute', component :NouveauteComponent ,
          children:[
            {path:'',component:ListeNouveauteComponent },
            {path:'ajouter',component:AjouterNouveauteComponent },
            {path:'modifier/:id',component:ModifierNouveauteComponent } ,
            {path:'detail/:id' , component:NouveauteDetailsComponent}         
          ]
        },
        {path:'emprunts',component:EmpruntsComponent ,
          children:[
            {path:'',component:ListeEmpruntsComponent } ,
            {path:'ajouter',component:AjoutEmpruntsComponent },
            {path:'modifier/:id',component:ModifierEmpruntsComponent },
          ]
        },
        {path:'livres',component:LivresComponent ,
          children:[
            {path:'',component:ListeLivresComponent } ,
            {path:'ajouter',component:AjoutLivresComponent },
            {path:'modifier/:id',component:ModifierLivresComponent }
          ]
        }
      ]
    } ,
    {path:'', redirectTo: 'accueil', pathMatch: 'full' },
    {path:'**',component:ErreurComponent ,pathMatch:'full'}
   
     
];
