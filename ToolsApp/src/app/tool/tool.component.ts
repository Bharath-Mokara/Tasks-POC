import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { DndDropEvent, DropEffect } from 'ngx-drag-drop';
import { Tool } from '../models/tool';
import { ActivatedRoute, Router } from '@angular/router';
import { TemplateService } from '../Services/template.service';
import { Template } from '../models/template';
declare var bootstrap: any;


@Component({
  selector: 'app-tool',
  templateUrl: './tool.component.html',
  styleUrls: ['./tool.component.css']
})
export class ToolComponent implements OnInit{

  title: string = "";

  dragTools: Array<Tool> = [
    {
      "type": "textbox",
      "icon": "bi-card-text",
      "label": "Text",
    },
    {
      "type": "textarea",
      "icon": "bi-textarea-resize",
      "label": "Textarea"
    },
    {
      "type": "dropdown",
      "icon": "bi-arrow-down-square-fill",
      "label": "Dropdown",
      "options" : []
    },
    {
      "type": "date",
      "icon": "bi-calendar-date-fill",
      "label": "Date Picker",
    },
    {
      "type": "table",
      "icon": "bi-table",
      "label": "Table",
    },
    {
      "type": "horizontal",
      "icon": "bi-line",
      "label": "Horizontal Line"
    }
  ];


  dropTools: Array<any> = [];
  dropdownOptions: string[] = [];
  tableRows: string = "";
  tableCols: string = "";
  tableData: any[][] = [];
  pendingEvent: DndDropEvent | null = null;
  pendingList: any[] | null | undefined = null;
  templateContent!: HTMLElement;

  
  constructor(private route:ActivatedRoute, private templateService: TemplateService) {
    
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      let canvasSection = document.getElementById('canvas-section');
      if(canvasSection && params['content']){
        canvasSection.innerHTML = params['content'];
        this.title = params['name'];
      }
    });

  }


  onDrop(event: DndDropEvent, list?: any[]) {

    const selection = window.getSelection();
    let range = null;
    if (selection && selection.rangeCount > 0) {
       range = selection.getRangeAt(0); 
    }

    if (event.data["type"] === "dropdown") {
      // Store the event and list for later use
      this.pendingEvent = event;
      this.pendingList = list;
      
      // Display a modal popup for adding options dynamically
      const modalElement = document.getElementById('myModal');
      const modal = new bootstrap.Modal(modalElement);
      modal.show();

      // Handle the modal hidden event
      modalElement?.addEventListener('hidden.bs.modal', this.handleModalHidden.bind(this,range), { once: true });
    } 
    else if(event.data["type"] === 'table')
    {
      this.pendingEvent = event;
      this.pendingList = list;

      const modalElement = document.getElementById('tableModal');
      const modal = new bootstrap.Modal(modalElement);
      modal.show();

      // Handle the modal hidden event
      modalElement?.addEventListener('hidden.bs.modal', this.handleModalHidden.bind(this,range), { once: true });
    }
    else 
    {
      this.processDrop(event, range,list);
    }

  }

  //Hiding the modal to prevent the execution 
  handleModalHidden(range?:any) {
    if (this.pendingEvent) {
        const list = this.pendingList || undefined;
        this.processDrop(this.pendingEvent,range, list);
        this.pendingEvent = null;
        this.pendingList = null;
    }
  }

  processDrop(event: DndDropEvent, range: any, list?: any[]) : void{

    let toolElement = null;
    switch(event.data["type"])
    {
      case 'textbox' : 
        toolElement = this.createTextBox();
        break;

      case 'textarea':
        toolElement = this.createTextarea();
        break;

      case 'date': 
        toolElement = this.createDatePicker();
        break;

      case 'dropdown':
        toolElement = this.createDropdown();
        break;
      case 'table':
        toolElement = this.createTable();
        break;
      case 'horizontal':
        toolElement = this.createHroizontal();
        break;
    }

    console.log(toolElement);

    // let content = this.getToolHTML(event.data);
    range.insertNode(toolElement);
    
  }


  getToolHTML(tool: Tool): string {
    switch (tool.type) {
      case 'textbox':
        return `<input placeholder="Enter text">`;
      case 'dropdown':
        tool.options = this.dropdownOptions;
        let optionsHTML = tool.options?.map(option => `<option value="${option}">${option}</option>`).join('');
        this.clearOptions();
        return `<select>${optionsHTML}</select>`;
      case 'date':
        return '<input type="date">';
      case 'textarea':
        return '<textarea placeholder="Enter your content"></textarea>';
      case 'table':
        let tableHTML = '<table contenteditable="true" border style="border-collapse: collapse;"><thead><tr border>';
        for (let i = 0; i < parseInt(this.tableCols); i++) {
          tableHTML += `<th class="p-1" style="border:1px solid black;">Column ${i + 1}</th>`;
        }
        tableHTML += '</tr></thead><tbody>';
    
        for (let row of this.tableData) {
          tableHTML += '<tr>';
          for (let cell of row) {
            tableHTML += `<td style="border:1px solid black" class="p-1">${cell}</td>`;
          }
          tableHTML += '</tr>';
        }
    
        tableHTML += '</tbody></table>';
    
        return tableHTML;
      case 'horizontal':
        return `<hr >`
      default:
        return '';
    }
  }

  addOption(inputElement: HTMLInputElement) {
    let value = inputElement.value.trim();
    if (value) {
      this.dropdownOptions.push(value);
      inputElement.value = '';
    }
  }

  clearOptions() {
    // Clearing the dropdownOptions list
    this.dropdownOptions.splice(0, this.dropdownOptions.length);
  }

  generateTable(rowsHtmlElement: HTMLInputElement,colsHtmlElement: HTMLInputElement)
  {
    let rows = rowsHtmlElement.value.trim();
    let cols = colsHtmlElement.value.trim();
    if(rows && cols)
    {
      this.tableRows = rows;
      this.tableCols = cols;
      rowsHtmlElement.value = '';
      colsHtmlElement.value = '';
    }

    this.tableData = [];
    for (let i = 0; i < parseInt(rows); i++) {
      const row = [];
      for (let j = 0; j < parseInt(cols); j++) {
        row.push(`edit`);
      }
      this.tableData.push(row);
    }
  }

  //Previewing the template that was rendered.

  showPreview()
  {
    const canvasSection = document.getElementById('canvas-section');
    canvasSection?.removeAttribute("contenteditable");
    this.templateContent = canvasSection as HTMLElement;

    // var extractedValues = this.extractContentValues(this.templateContent);
    // console.log(extractedValues);
    // let templateAfterValueRemoval = this.removeContentValues(this.templateContent.outerHTML,extractedValues);
    // console.log(templateAfterValueRemoval);
    // console.log("-------------------------------------------------");
    // let finalTemplateContent = this.updateContentValues(templateAfterValueRemoval,extractedValues);

    // console.log(finalTemplateContent);

    // if(canvasSection)
    // {
    //   canvasSection.innerHTML = "";
    //   canvasSection.innerHTML = finalTemplateContent;
    // }


    // Display a modal popup to preview the template
    const modalElement = document.getElementById('previewModal');
    const modal = new bootstrap.Modal(modalElement);
    modal.show();

    const previewModalBody = document.getElementById("previewModalBody");
    if(previewModalBody)
    {
      previewModalBody.innerHTML = '';
      
      previewModalBody.appendChild(this.templateContent.cloneNode(true) as HTMLElement);
    }

    if(canvasSection !== null)
    {
      // Handle the modal hidden event
      modalElement?.addEventListener('hidden.bs.modal', this.handlePreviewModalHidden.bind(this,canvasSection), { once: true });
    }
    
  }

  handlePreviewModalHidden(canvasSection:HTMLElement)
  {
    canvasSection?.setAttribute("contenteditable",'true');
  }


  //Tools creation

  createTextBox() : any
  {
    const textBoxCount = this.dropTools.filter(el => el.type === 'textbox').length + 1;
    const textBoxNode = document.createElement('input');
    textBoxNode.setAttribute('type', 'text');
    textBoxNode.setAttribute('placeholder', 'Textbox');
    textBoxNode.setAttribute('id', `textbox-${textBoxCount}`);
    textBoxNode.setAttribute('name', `textbox-${textBoxCount}`);
    this.dropTools.push({ id: `textbox-${textBoxCount}`, type: 'textbox' });
    return textBoxNode;
  }

  createTextarea() : any
  {
    const textAreaCount = this.dropTools.filter(el => el.type === 'textarea').length + 1;
    const textAreaNode = document.createElement('textarea');
    textAreaNode.setAttribute('placeholder', 'Textarea');
    textAreaNode.setAttribute('id', `textarea-${textAreaCount}`);
    textAreaNode.setAttribute('name', `textarea-${textAreaCount}`);
    this.dropTools.push({ id: `textarea-${textAreaCount}`, type: 'textarea' });
    return textAreaNode;;
  }

  createDatePicker() : any
  {
    const datePickerCount = this.dropTools.filter(el => el.type === 'datepicker').length + 1;
    const datePickerNode = document.createElement('input');
    datePickerNode.setAttribute('type', 'date');
    datePickerNode.setAttribute('placeholder', 'dd/MM/yyyy');
    datePickerNode.setAttribute('id', `datepicker-${datePickerCount}`);
    datePickerNode.setAttribute('name', `datepicker-${datePickerCount}`);
    this.dropTools.push({ id: `datepicker-${datePickerCount}`, type: 'datepicker' });
    return datePickerNode;
  }


  createDropdown() : any
  {
    const dropDownCount = this.dropTools.filter(el => el.type === 'dropdown').length + 1;
    const dropDownNode = document.createElement('select');
    const defaultOption = document.createElement('option');
    defaultOption.setAttribute('value', 'Default');
    dropDownNode.setAttribute('id', `dropdown-${dropDownCount}`);
    dropDownNode.setAttribute('name', `dropdown-${dropDownCount}`);
    defaultOption.textContent = 'select item';
    dropDownNode.appendChild(defaultOption);

    this.dropdownOptions.forEach(option => {
      let optionElement = document.createElement('option');
      optionElement.setAttribute('value',option);
      optionElement.textContent = option;
      dropDownNode.appendChild(optionElement);
    });

    this.dropTools.push({ id: `dropdown-${dropDownCount}`, type: 'dropdown' });

    return dropDownNode;
  }


  createTable() : any
  {
    const tableNode = document.createElement('table');
    tableNode.style.borderCollapse = 'collapse';

    // Create thead
    const thead = tableNode.createTHead();
    const headerRow = thead.insertRow(0);
    for (let j = 0; j < parseInt(this.tableCols); j++) {
      const header = document.createElement('th');
      header.textContent = `column ${j + 1}`;
      header.style.border = '1px solid black';
      headerRow.appendChild(header);
    }

    // Create tbody
    const tbody = tableNode.createTBody();
    for (let i = 0; i < parseInt(this.tableRows); i++) {
      const bodyRow = tbody.insertRow(i);
      for (let j = 0; j < parseInt(this.tableCols); j++) {
        const cell = bodyRow.insertCell(j); 
        cell.textContent = 'data';
        cell.style.border = '1px solid black';
      }
    }

    return tableNode;
    
  }

  createHroizontal() : any
  {
    const horizontalNode = document.createElement('hr');
    return horizontalNode;
  }


  // Extracting the elements values 

  extractContentValues(container: HTMLElement): { [key: string]: any } {
    const values: { [key: string]: any } = {};
    const inputs = container.querySelectorAll('input, select, textarea');

    inputs.forEach(input => {
      if (input instanceof HTMLInputElement || input instanceof HTMLTextAreaElement || input instanceof HTMLSelectElement) {
        values[input.name] = input.value;
      }
    });

    return values;
  }

  //Updating the extracted values to corresponding HTML elements to store final HTML in DB with user entered values.

  updateContentValues(htmlString: string, values: { [key: string]: any }) : string{
    const container = document.createElement('div');
    container.innerHTML = htmlString;

    for (const name in values) {
      const value = values[name];
      const input = container.querySelector(`[name="${name}"]`);

      if (input instanceof HTMLInputElement){
        input.setAttribute("value",value);
      } 
      else if(input instanceof HTMLTextAreaElement)
      {
        input.innerText = value;
      }
      else if (input instanceof HTMLSelectElement) {
        Array.from(input.options).forEach(option => {
          if (option.value === value) {
            option.setAttribute("selected","true");
          }
        });
      }
    }

    return container.innerHTML;
  }

  removeContentValues(htmlString: string, values: { [key: string]: any }) : string{
    const container = document.createElement('div');
    container.innerHTML = htmlString;

    for (const name in values) {
      const value = values[name];
      const input = container.querySelector(`[name="${name}"]`);

      if (input instanceof HTMLInputElement){
        input.removeAttribute("value");
      } 
      else if(input instanceof HTMLTextAreaElement)
      {
        input.innerText = value;
      }
      else if (input instanceof HTMLSelectElement) {
        Array.from(input.options).forEach(option => {
          option.removeAttribute("Selected");
        });
      }
    }

    return container.innerHTML;
  }

  //Saving the final HTML with values into database

  PublishTemplate()
  {
    const canvasSection = document.getElementById('canvas-section');
    this.templateContent = canvasSection as HTMLElement;
    console.log(this.templateContent.innerHTML);
    
    var extractedValues = this.extractContentValues(this.templateContent);
    console.log(extractedValues);
    let finalTemplateContent = this.updateContentValues(this.templateContent.outerHTML,extractedValues);


    const template = new Template(this.title, finalTemplateContent);

    this.templateService.addTemplate(template).subscribe({
      next:(response:Response)=>{
        console.log(response);
        if(canvasSection)
        {
          canvasSection.innerHTML = "";
        }
      }
    });
  }
  

}
