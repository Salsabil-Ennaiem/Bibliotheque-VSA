import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class ProductService {
    getProductsData() {
        return [
            {
                id: '1000',
                code: 'f230fh0g3',
                titre: 'Bamboo Watch',
                description: 'Product Description',
                fichier: '',
                image: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ft3.ftcdn.net%2Fjpg%2F00%2F49%2F33%2F98%2F360_F_49339849_hXEJkwS3nBXO7Xeeztxw8fcEGzEEi9qG.jpg&f=1&nofb=1&ipt=60f03f85c6ff24c2ce5f546a6bbb9d0d8a68219d71992fef40fcdd655696dea5',
                date_pub: '2000-01-01',
            },
            {
                id: '1001',
                code: 'nvklal433',
                titre: 'Black Watch',
                fichier: '',
                description: 'Product Description',
                image: 'https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ft3.ftcdn.net%2Fjpg%2F00%2F49%2F33%2F98%2F360_F_49339849_hXEJkwS3nBXO7Xeeztxw8fcEGzEEi9qG.jpg&f=1&nofb=1&ipt=60f03f85c6ff24c2ce5f546a6bbb9d0d8a68219d71992fef40fcdd655696dea5',
                date_pub: '2025-01-01',
            },
            {
                id: '1002',
                code: 'zz21cz3c1',
                titre: 'Blue Band',
                fichier: '',
                description: 'Product Description',
                image: 'blue-band.jpg',
                date_pub: '2022-01-01',
            }]
    }


    getProductsMini() {
        return Promise.resolve(this.getProductsData().slice(0, 5));
    }

    getProductsSmall() {
        return Promise.resolve(this.getProductsData().slice(0, 10));
    }

    getProducts() {
        return Promise.resolve(this.getProductsData());
    }



}
