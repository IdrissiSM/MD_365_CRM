using MD_365_CRM.Requests;

namespace MD_365_CRM.Models
{
    public class Incident
    {
        public Guid incidentid { get; set; }
        public string? title { get; set; }
        /// <summary>
        /// Sélectionnez la phase de l'incident dans le processus de résolution de l'incident.
        /// 0 => Identify, 1 => Research, 2 => Resolve
        /// </summary>
        public Int32? servicestage { get; set; }
        /// <summary>
        /// Entrez la date maximale à laquelle l'incident doit être résolu.
        /// </summary>
        public DateTimeOffset? resolveby { get; set; }
        /// <summary>
        /// Entrez des informations supplémentaires pour décrire l'incident et aider l'équipe du service clientèle à trouver une solution.
        /// </summary>
        public string? description { get; set; }
        /// <summary>
        /// Indique si l'incident est actif, résolu ou annulé. Les incidents résolus et annulés sont en lecture seule et ne peuvent pas être modifiés avant d'être réactivés.
        /// 0 => Active, 1 => Resolved, 2 => Canceled
        /// </summary>
        public Int32 statecode { get; set; }
        /// <summary>
        /// Sélectionnez l'origine du contact à propos de l'incident, par exemple courrier électronique, téléphone ou web, en vue d'une utilisation dans la génération de rapports et d'analyses.
        /// 1 => Phone, 2 => Email, 3 => Web, 2483 => Facebook, 3986 => Twitter
        /// </summary>
        public Int32? caseorigincode { get; set; }
        /// <summary>
        /// Select the type of case to identify the incident for use in case routing and analysis.
        /// 1 => Question, 2 => Problem, 3 => Request
        /// </summary>
        public Int32? casetypecode { get; set; }
        /// <summary>
        /// Tapez le numéro de série du produit associé à cet incident, pour pouvoir indiquer le nombre d'incidents par produit.
        /// </summary>
        public string? productserialnumber { get; set; }
        /// <summary>
        /// Date and time when the record was created.
        /// </summary>
        public DateTimeOffset? createdon { get; set; }
        /// <summary>
        /// Indique si un conseiller du service clientèle a contacté ou non le client.
        /// </summary>
        public bool? customercontacted { get; set; }
        /// <summary>
        /// Identificateur unique du contact associé à l'incident.
        /// </summary>
        public Guid? _contactid_value { get; set; }
        /// <summary>
        /// Choisissez le produit associé à l'incident pour identifier la garantie, le service ou tout autre problème lié au produit et être en mesure d'indiquer le nombre d'incidents pour chaque produit.
        /// </summary>
        public Guid? _productid_value { get; set; }
        /// <summary>
        /// Select the customer account or contact to provide a quick link to additional customer details, such as account information, activities, and opportunities.
        /// </summary>
        public Guid _customerid_value { get; set; }
        /// <summary>
        /// Affiche le numéro d'incident pour servir de référence client et permettre les recherches. Vous ne pouvez pas le modifier.
        /// </summary>
        public string? ticketnumber { get; set; }
        /// <summary>
        /// Affiche la date et l’heure de résolution de l’incident.
        /// </summary>
        public DateTimeOffset? deactivatedon { get; set; }
        /// <summary>
        /// Indique si l'incident a été acheminé ou non vers la file d'attente.
        /// </summary>
        public bool? routecase { get; set; }
        /// <summary>
        /// Sélectionnez la priorité afin que les clients préférés ou les problèmes critiques soient traités rapidement.
        /// 1 => High, 2 => Normal, 3 => Low
        /// </summary>
        public Int32? prioritycode { get; set; }
        public ContactDTO? customerid_contact { get; set; }

        public static string Properties()
        {
            return "incidentid,title,servicestage,resolveby,description,statecode,caseorigincode,productserialnumber,createdon,customercontacted,_contactid_value,_productid_value,ticketnumber,deactivatedon,routecase,prioritycode,_customerid_value,casetypecode";
        }
    }
}
