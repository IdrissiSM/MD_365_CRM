
export enum ServicesStage {
    IDENTIFY = 0,
    RESEARCH = 1,
    RESOLVE = 2
}

export enum StateCode {
    ACTIVE = 0,
    RESOLVED = 1,
    CANCELLED = 2
}

export  enum  CaseOriginCode {
    PHONE = 1,
    EMAIL= 2,
    WEB = 3,
    FACEBOOK= 2483,
    TWITTER= 3986
}

export enum PriorityCode {
    HIGH = 1,
    NORMAL = 2,
    LOW = 3
}
export enum CaseTypeCode {
    QUESTION = 1,
    PROBLEM = 2,
    REQUEST = 3
}

export enum RouteCase {

}
export class Incident {
    public incidentid: string;
    public title: string;
    /**
     * Sélectionnez la phase de l'incident dans le processus de résolution de l'incident.
     * 0 => Identify, 1 => Research, 2 => Resolve
     */
    public servicestage: number | null;
    /**
     * Entrez la date maximale à laquelle l'incident doit être résolu.
     */
    public resolveby: Date | null;
    /**
     * Entrez des informations supplémentaires pour décrire l'incident et aider l'équipe du service clientèle à trouver une solution.
     */
    public description: string | null;
    /**
     * Indique si l'incident est actif, résolu ou annulé. Les incidents résolus et annulés sont en lecture seule et ne peuvent pas être modifiés avant d'être réactivés.
     * 0 => Active, 1 => Resolved, 2 => Canceled
     */
    public statecode: number;
    /**
     * Sélectionnez l'origine du contact à propos de l'incident, par exemple courrier électronique, téléphone ou web, en vue d'une utilisation dans la génération de rapports et d'analyses.
     * 1 => Phone, 2 => Email, 3 => Web, 2483 => Facebook, 3986 => Twitter
     */
    public caseorigincode: number | null;
    /**
    * Select the type of case to identify the incident for use in case routing and analysis.
    *  1 => Question, 2 => Problem, 3 => Request
    */
    public casetypecode: number | null;
    /**
     * Tapez le numéro de série du produit associé à cet incident, pour pouvoir indiquer le nombre d'incidents par produit.
     */
    public productserialnumber: string | null;
    /**
     * Date and time when the record was created.
     */
    public createdon: Date | null;
    /**
     * Indique si un conseiller du service clientèle a contacté ou non le client.
     */
    public customercontacted: boolean | null;
    /**
     * Identificateur unique du contact associé à l'incident.
     */
    public _contactid_value: string | null;
    /**
     * Choisissez le produit associé à l'incident pour identifier la garantie, le service ou tout autre problème lié au produit et être en mesure d'indiquer le nombre d'incidents pour chaque produit.
     */
    public _productid_value: string | null;
    public _customerid_value : string | null;
    /**
     * Affiche le numéro d'incident pour servir de référence client et permettre les recherches. Vous ne pouvez pas le modifier.
     */
    public ticketnumber: string | null;
    /**
     * Affiche la date et l’heure de résolution de l’incident.
     */
    public deactivatedon: Date | null;
    /**
     * Indique si l'incident a été acheminé ou non vers la file d'attente.
     */
    public routecase: boolean | null;
    /**
     * Sélectionnez la priorité afin que les clients préférés ou les problèmes critiques soient traités rapidement.
     * 1 => High, 2 => Normal, 3 => Low
     */
    public prioritycode: number | null;

    constructor(
        incidentid : string,
        title : string,
        servicestage : number,
        resolveby : Date,
        description : string,
        statecode : number,
        caseorigincode : number,
        productserialnumber : string,
        createdon : Date,
        customercontacted : boolean,
        _contactid_value : string,
        _productid_value : string,
        ticketnumber : string,
        deactivatedon : Date,
        routecase : boolean,
        prioritycode : number,
        _customerid_value : string,
        casetypecode : number
    ) {
        this.incidentid = incidentid;
        this.title = title;
        this.servicestage = servicestage;
        this.resolveby = resolveby;
        this.description = description;
        this.statecode = statecode;
        this.caseorigincode = caseorigincode;
        this.productserialnumber = productserialnumber;
        this.createdon = createdon;
        this.customercontacted = customercontacted;
        this._contactid_value = _contactid_value;
        this._productid_value = _productid_value;
        this.ticketnumber = ticketnumber;
        this.deactivatedon = deactivatedon;
        this.routecase = routecase;
        this.prioritycode = prioritycode;
        this._customerid_value = _customerid_value;
        this.casetypecode = casetypecode;
    }
}
