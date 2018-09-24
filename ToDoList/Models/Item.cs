using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ToDoList;
using System;

namespace ToDoList.Models
{
  public class Item
  {

    public int id {get; set; }
    public string description {get; set; }
    public string dueDate {get; set; }
    public string status {get; set; }

    public Item(string Description, string newDueDate, int Id = 0)
    {
      id = Id;
      description = Description;
      dueDate = newDueDate;
      status = status;
    }

    public void Edit(string newDescription, string newDueDate, string newStatus)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE items SET description = @newDescription WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      MySqlParameter itemDescription = new MySqlParameter();
      itemDescription.ParameterName = "@newDescription";
      itemDescription.Value = newDescription;
      cmd.Parameters.Add(itemDescription);

      MySqlParameter itemDueDate = new MySqlParameter();
      itemDueDate.ParameterName = "@newDueDate";
      itemDueDate.Value = newDescription;
      cmd.Parameters.Add(itemDueDate);

      MySqlParameter itemStatus = new MySqlParameter();
      itemStatus.ParameterName = "@newStatus";
      itemStatus.Value = newDescription;
      cmd.Parameters.Add(itemStatus);


      cmd.ExecuteNonQuery();
      description = newDescription;

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM items WHERE id = @ItemId; DELETE FROM categories_items WHERE item_id = @ItemId;";

      MySqlParameter itemIdParameter = new MySqlParameter();
      itemIdParameter.ParameterName = "@ItemId";
      itemIdParameter.Value = id;
      cmd.Parameters.Add(itemIdParameter);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
    // public void Flip()
    // {
    //   if (done)
    //   {
    //     done = false;
    //   }
    //   else
    //   {
    //     done = true;
    //   }
    //
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //   var cmd = conn.CreateCommand() as MySqlCommand;
    //   cmd.CommandText = @"UPDATE items SET status = @newStatus WHERE id = @searchId;";
    //
    //   MySqlParameter searchId = new MySqlParameter();
    //   searchId.ParameterName = "@searchId";
    //   searchId.Value = id;
    //   cmd.Parameters.Add(searchId);
    //
    //   MySqlParameter itemStatus = new MySqlParameter();
    //   itemStatus.ParameterName = "@newStatus";
    //   itemStatus.Value = done;
    //   cmd.Parameters.Add(itemStatus);
    //
    //   cmd.ExecuteNonQuery();
    //
    //   conn.Close();
    //   if (conn != null)
    //   {
    //       conn.Dispose();
    //   }
    // }

    public static List<Item> GetAll()
    {
        List<Item> allItems = new List<Item> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM items;";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int itemId = rdr.GetInt32(0);
          string itemDescription = rdr.GetString(1);
          string itemDueDate = rdr.GetString(2);
          Item newItem = new Item(itemDescription, itemDueDate, itemId);
          allItems.Add(newItem);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return allItems;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO items (description, dueDate) VALUES (@itemDescription, @dueDate);";

      MySqlParameter Description = new MySqlParameter();
      Description.ParameterName = "@itemDescription";
      Description.Value = this.description;
      cmd.Parameters.Add(Description);

      MySqlParameter DueDate = new MySqlParameter();
      DueDate.ParameterName = "@dueDate";
      DueDate.Value = this.dueDate;
      cmd.Parameters.Add(DueDate);

      cmd.ExecuteNonQuery();
      id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn !=null)
      {
        conn.Dispose();
      }
    }

    public void Status(string status)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO items (status) VALUES (@Status);";

      MySqlParameter Status = new MySqlParameter();
      Status.ParameterName = "@Status";
      Status.Value = this.status;
      cmd.Parameters.Add(Status);

      cmd.ExecuteNonQuery();
      id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn !=null)
      {
        conn.Dispose();
      }
    }


    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM items;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is Item))
      {
        return false;
      }
      else
      {

        Item newItem = (Item) otherItem;
        bool idEquality = (this.id == newItem.id);
        bool descriptionEquality = (this.description == newItem.description);
        bool dueDateEquality = (this.dueDate == newItem.dueDate);
        return (descriptionEquality && idEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.description.GetHashCode();
    }

    public static Item Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `items` WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int itemId = 0;
      string itemDescription = "";
      string itemDueDate = "";

      while (rdr.Read())
      {
          itemId = rdr.GetInt32(0);
          itemDescription = rdr.GetString(1);
          itemDueDate = rdr.GetString(2);

      }

      Item foundItem= new Item(itemDescription, itemDueDate, itemId);  // This line is new!

       conn.Close();
       if (conn != null)
       {
           conn.Dispose();
       }

      return foundItem;  // This line is new!
    }

      public void AddCategory(Category newCategory)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO categories_items (category_id, item_id) VALUES (@CategoryId, @ItemId);";

        MySqlParameter category_id = new MySqlParameter();
        category_id.ParameterName = "@CategoryId";
        category_id.Value = newCategory.GetId();
        cmd.Parameters.Add(category_id);

        MySqlParameter item_id = new MySqlParameter();
        item_id.ParameterName = "@ItemId";
        item_id.Value = id;
        cmd.Parameters.Add(item_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
      }
      public List<Category> GetCategories()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT category_id FROM categories_items WHERE item_id = @itemId;";

          MySqlParameter itemIdParameter = new MySqlParameter();
          itemIdParameter.ParameterName = "@itemId";
          itemIdParameter.Value = id;
          cmd.Parameters.Add(itemIdParameter);

          var rdr = cmd.ExecuteReader() as MySqlDataReader;

          List<int> categoryIds = new List<int> {};
          while(rdr.Read())
          {
              int categoryId = rdr.GetInt32(0);
              categoryIds.Add(categoryId);
          }
          rdr.Dispose();

          List<Category> categories = new List<Category> {};
          foreach (int categoryId in categoryIds)
          {
              var categoryQuery = conn.CreateCommand() as MySqlCommand;
              categoryQuery.CommandText = @"SELECT * FROM categories WHERE id = @CategoryId;";

              MySqlParameter categoryIdParameter = new MySqlParameter();
              categoryIdParameter.ParameterName = "@CategoryId";
              categoryIdParameter.Value = categoryId;
              categoryQuery.Parameters.Add(categoryIdParameter);

              var categoryQueryRdr = categoryQuery.ExecuteReader() as MySqlDataReader;
              while(categoryQueryRdr.Read())
              {
                  int thisCategoryId = categoryQueryRdr.GetInt32(0);
                  string categoryName = categoryQueryRdr.GetString(1);
                  Category foundCategory = new Category(categoryName, thisCategoryId);
                  categories.Add(foundCategory);
              }
              categoryQueryRdr.Dispose();
          }
          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
          return categories;
        }
    }

}
