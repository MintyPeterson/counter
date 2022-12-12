import 'dart:convert';
import 'package:counter/services/authorization_service.dart';
import 'package:counter/services/counter/requests/entry_delete_request.dart';
import 'package:counter/services/counter/requests/entry_edit_request.dart';
import 'package:counter/services/counter/requests/entry_new_request.dart';
import 'package:counter/services/counter/requests/entry_view_request.dart';
import 'package:counter/services/counter/responses/entry_delete_response.dart';
import 'package:counter/services/counter/responses/entry_edit_response.dart';
import 'package:counter/services/counter/responses/entry_list_group_response.dart';
import 'package:counter/services/counter/responses/entry_list_response.dart';
import 'package:counter/services/counter/responses/entry_new_response.dart';
import 'package:counter/services/counter/responses/entry_view_response.dart';
import 'package:http/http.dart' as http;

/// A service for interacting with the counter API.
class CounterService {

  static const String counterApiUrl = 'https://mintypeterson-counter-api.azurewebsites.net';

  final AuthorizationService authorizationService;
  CounterService(this.authorizationService);

  Future<EntryListResponse> entryList() async {
    final String? accessToken = await authorizationService.getValidAccessToken();
    final http.Response response = await http.get(
      Uri.parse('$counterApiUrl/Entries'),
      headers: <String, String>{
        'Content-Type': 'application/json',
        'Authorization': 'Bearer $accessToken',
      });
    if (response.statusCode == 200) {
      final List<EntryListGroupResponse> groups =
        List<EntryListGroupResponse>.from(
          json.decode(response.body)['groups'].map((json) => EntryListGroupResponse.fromJson(json)));
      return EntryListResponse(groups.toList());
    }
    throw Exception('Could not get the entries');
  }

  Future<EntryNewResponse> entryNew(EntryNewRequest entry) async {
    final String? accessToken = await authorizationService.getValidAccessToken();
    final http.Response response = await http.post(
      Uri.parse('$counterApiUrl/Entry'),
      headers: <String, String>{
        'Content-Type': 'application/json',
        'Authorization': 'Bearer $accessToken',
      },
      body: jsonEncode(entry.toJson()));
    if (response.statusCode == 200) {
      return EntryNewResponse.fromJson(json.decode(response.body));
    }
    throw Exception('Could not add entry');
  }

  Future<EntryDeleteResponse> entryDelete(EntryDeleteRequest entry) async {
    final String? accessToken = await authorizationService.getValidAccessToken();
    final http.Response response = await http.delete(
      Uri.parse('$counterApiUrl/Entry/${entry.entryId}'),
      headers: <String, String>{
        'Content-Type': 'application/json',
        'Authorization': 'Bearer $accessToken',
      },
    );
    if (response.statusCode == 200) {
      return EntryDeleteResponse.fromJson(json.decode(response.body));
    }
    throw Exception('Could not delete entry');
  }

  Future<EntryEditResponse> entryEdit(EntryEditRequest entry) async {
    final String? accessToken = await authorizationService.getValidAccessToken();
    final http.Response response = await http.put(
      Uri.parse('$counterApiUrl/Entry/${entry.entryId}'),
      headers: <String, String>{
        'Content-Type': 'application/json',
        'Authorization': 'Bearer $accessToken',
      },
      body: jsonEncode(entry.toJson()));
    if (response.statusCode == 200) {
      return EntryEditResponse.fromJson(json.decode(response.body));
    }
    throw Exception('Could not edit entry');
  }

  Future<EntryViewResponse> entryView(EntryViewRequest entry) async {
    final String? accessToken = await authorizationService.getValidAccessToken();
    final http.Response response = await http.get(
      Uri.parse('$counterApiUrl/Entry/${entry.entryId}'),
      headers: <String, String>{
        'Content-Type': 'application/json',
        'Authorization': 'Bearer $accessToken',
      });
    if (response.statusCode == 200) {
      return EntryViewResponse.fromJson(json.decode(response.body));
    }
    throw Exception('Could not get the entry');
  }
}