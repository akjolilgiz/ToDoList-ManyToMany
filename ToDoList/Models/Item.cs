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

    public Item(string Description, int Id = 0)
    {
      id = Id;
      description = Description;
    }

    public void Edit(string newDescription, int newCategoryId)
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
      cmd.CommandText = @"DELETE FROM items WHERE id = @thisId; DELETE FROM categories_items WHERE item_id = @ItemId;";

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
          Item newItem = new Item(itemDescription, itemId);
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
      cmd.CommandText = @"INSERT INTO items (description) VALUES (@itemDescription);";

      MySqlParameter Description = new MySqlParameter();
      Description.ParameterName = "@itemDescription";
      Description.Value = this.description;
      cmd.Parameters.Add(Description);

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

      while (rdr.Read())
      {
          itemId = rdr.GetInt32(0);
          itemDescription = rdr.GetString(1);

      }

      Item foundItem= new Item(itemDescription, itemId);  // This line is new!

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
